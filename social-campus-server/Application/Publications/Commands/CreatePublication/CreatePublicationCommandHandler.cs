using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.CreatePublication;

public class CreatePublicationCommandHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository,
    IStorageService storageService,
    ITagRepository tagRepository,
    IPublicationTagRepository publicationTagRepository) : ICommandHandler<CreatePublicationCommand>
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
            var uploadStream = request.ImageData.Content;
            imageUrl = await storageService.UploadAsync(
                uploadStream,
                request.CreatorId,
                "publication",
                request.ImageData.ContentType,
                cancellationToken);
        }

        var createdPublication =
            await publicationRepository.AddAsync(request.Description, request.CreatorId, imageUrl);

        var labels = TagHelpers.ExtractLabels(createdPublication.Description);

        foreach (var label in labels)
        {
            var tag = await tagRepository.GetByLabelAsync(label)
                      ?? await tagRepository.AddAsync(label, cancellationToken);

            await publicationTagRepository.AddAsync(tag.Id, createdPublication.Id, cancellationToken);
        }

        return Result.Success();
    }
}