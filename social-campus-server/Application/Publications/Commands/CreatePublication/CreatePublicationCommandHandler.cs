using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.CreatePublication;

public class CreatePublicationCommandHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository,
    IStorageService storageService) : ICommandHandler<CreatePublicationCommand>
{
    public async Task<Result> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = await userRepository.IsExistByIdAsync(request.CreatorId);
        if (!isUserExist)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with id {request.CreatorId.Value} was not found"));

        var imageUrl = "";
        if (request.ImageData != null)
        {
            await using var uploadStream = request.ImageData.OpenReadStream();
            imageUrl = await storageService.UploadAsync(
                uploadStream,
                request.CreatorId,
                "publication",
                request.ImageData.ContentType,
                cancellationToken);
        }

        await publicationRepository.AddAsync(request.Description, request.CreatorId, imageUrl);

        return Result.Success();
    }
}