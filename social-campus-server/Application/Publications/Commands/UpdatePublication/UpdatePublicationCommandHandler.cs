using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Dtos;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.UpdatePublication;

public class UpdatePublicationCommandHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository,
    IStorageService storageService,
    ITagRepository tagRepository,
    IPublicationTagRepository publicationTagRepository) : ICommandHandler<UpdatePublicationCommand>
{
    public async Task<Result> Handle(UpdatePublicationCommand request, CancellationToken cancellationToken)
    {
        var publication = await publicationRepository.GetByIdAsync(request.PublicationId);
        if (publication is null)
            return Result.Failure<PublicationDto>(new Error(
                "Publication.NotFound",
                $"Publication with PublicationId {request.PublicationId.Value} was not found"));

        if (!await userRepository.IsExistByIdAsync(request.CallerId))
            return Result.Failure(new Error(
                "User.NotFound",
                $"Caller with id {request.CallerId.Value} was not found"));

        if (publication.CreatorId != request.CallerId)
            return Result.Failure<PublicationDto>(new Error(
                "User.NoUpdatePermission",
                $"User with UserId {request.CallerId.Value} do not have permission to update publication with PublicationId {request.PublicationId.Value}"));

        var imageUrl = publication.ImageUrl;
        if (request.ImageData is null)
        {
            if (!string.IsNullOrEmpty(imageUrl)) await storageService.DeleteAsync(imageUrl, cancellationToken);
            imageUrl = null;
        }
        else
        {
            var newImageStream = request.ImageData.Content;
            var newHash = await StorageHelpers.ComputeHashAsync(newImageStream);
            newImageStream.Position = 0;

            var shouldUpload = true;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                using var oldStream = new MemoryStream();
                await storageService.DownloadAsync(imageUrl, oldStream, cancellationToken);
                oldStream.Position = 0;

                var oldHash = await StorageHelpers.ComputeHashAsync(oldStream);
                shouldUpload = oldHash != newHash;
            }

            if (shouldUpload)
            {
                if (!string.IsNullOrEmpty(imageUrl)) await storageService.DeleteAsync(imageUrl, cancellationToken);

                var uploadStream = request.ImageData.Content;
                imageUrl = await storageService.UploadAsync(
                    uploadStream,
                    publication.CreatorId,
                    "publication",
                    request.ImageData.ContentType,
                    cancellationToken);
            }
        }

        publicationRepository.Update(publication, request.Description, imageUrl ?? "");

        var newLabels = TagHelpers.ExtractLabels(request.Description);
        var currentTags = await publicationTagRepository.GetTagsForPublicationAsync(publication.Id, cancellationToken);
        var currentLabels = currentTags.Select(t => t.Label.ToLowerInvariant()).ToHashSet();

        var removedLabels = currentLabels.Except(newLabels).ToList();
        foreach (var label in removedLabels)
        {
            var tag = currentTags.First(t => t.Label.Equals(label, StringComparison.OrdinalIgnoreCase));
            await publicationTagRepository.RemoveAsync(tag.Id, publication.Id, cancellationToken);
        }

        var addedLabels = newLabels.Except(currentLabels).ToList();
        foreach (var label in addedLabels)
        {
            var tag = await tagRepository.GetByLabelAsync(label)
                      ?? await tagRepository.AddAsync(label, cancellationToken);

            await publicationTagRepository.AddAsync(tag.Id, publication.Id, cancellationToken);
        }

        return Result.Success();
    }
}