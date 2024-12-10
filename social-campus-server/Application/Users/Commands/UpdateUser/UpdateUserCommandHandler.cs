using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository userRepository) : ICommandHandler<UpdateUserCommand>
    {
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with id {request.UserId.Value} was not found"));
            }

            if (!await userRepository.IsExistByIdAsync(request.CallerId))
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"Caller with id {request.CallerId.Value} was not found"));
            }

            if (user.Id != request.CallerId)
            {
                return Result.Failure<ShortPublicationDto>(new Error(
                    "User.NoUpdatePermission",
                    $"User with UserId {request.CallerId.Value} do not have permission to updatep profile of user with UserId {request.UserId.Value}"));
            }

            bool isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email);
            if (!isEmailUnique && request.Email != user.Email)
            {
                return Result.Failure(new Error(
                "User.NotUniqueEmail",
                $"User with email {request.Email} has already exist"));
            }

            bool isLoginUnique = await userRepository.IsLoginUniqueAsync(request.Login);
            if (!isLoginUnique && request.Login != user.Login)
            {
                return Result.Failure(new Error(
                "User.NotUniqueLogin",
                $"User with login {request.Login} has already exist"));
            }

            userRepository.Update(
                user,
                request.Login,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.ProfileImageData);

            return Result.Success();
        }
    }
}
