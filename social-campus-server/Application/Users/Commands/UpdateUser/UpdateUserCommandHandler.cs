﻿using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IStorageService storageService) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with id {request.UserId.Value} was not found"));

        if (!await userRepository.IsExistByIdAsync(request.CallerId))
            return Result.Failure(new Error(
                "User.NotFound",
                $"Caller with id {request.CallerId.Value} was not found"));

        if (user.Id != request.CallerId)
            return Result.Failure(new Error(
                "User.NoUpdatePermission",
                $"User with UserId {request.CallerId.Value} do not have permission to update profile of user with UserId {request.UserId.Value}"));

        var isLoginUnique = await userRepository.IsLoginUniqueAsync(request.Login);
        if (!isLoginUnique && request.Login != user.Login)
            return Result.Failure(new Error(
                "User.NotUniqueLogin",
                $"User with login {request.Login} has already exist"));

        var imageObjectKey = user.ProfileImageObjectKey;
        if (request.ProfileImage is null)
        {
            if (!string.IsNullOrEmpty(imageObjectKey)) await storageService.DeleteAsync(imageObjectKey, cancellationToken);
            imageObjectKey = null;
        }
        else
        {
            var newImageStream = request.ProfileImage.Content;
            var newHash = await StorageHelpers.ComputeHashAsync(newImageStream);
            newImageStream.Position = 0;

            var shouldUpload = true;
            if (!string.IsNullOrEmpty(imageObjectKey))
            {
                using var oldStream = new MemoryStream();
                await storageService.DownloadAsync(imageObjectKey, oldStream, cancellationToken);
                oldStream.Position = 0;

                var oldHash = await StorageHelpers.ComputeHashAsync(oldStream);
                shouldUpload = oldHash != newHash;
            }

            if (shouldUpload)
            {
                if (!string.IsNullOrEmpty(imageObjectKey)) await storageService.DeleteAsync(imageObjectKey, cancellationToken);

                var uploadStream = request.ProfileImage.Content;
                imageObjectKey = await storageService.UploadAsync(
                    uploadStream,
                    user.Id,
                    "profile",
                    request.ProfileImage.ContentType,
                    cancellationToken);
            }
        }

        userRepository.Update(
            user,
            request.Login,
            request.FirstName,
            request.LastName,
            request.Bio,
            imageObjectKey ?? "");

        return Result.Success();
    }
}