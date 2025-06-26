using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.DeletePublication;

public class DeletePublicationCommandHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository,
    IStorageService storageService) : ICommandHandler<DeletePublicationCommand>
{
    public async Task<Result> Handle(DeletePublicationCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.IsExistByIdAsync(request.CallerId))
            return Result.Failure(new Error(
                "User.NotFound",
                $"Caller with id {request.CallerId.Value} was not found"));

        var publication = await publicationRepository.GetByIdAsync(request.PublicationId);
        if (publication is null)
            return Result.Failure(new Error(
                "Publication.NotFound",
                $"Publication with PublicationId {request.PublicationId.Value} was not found"));

        if (publication.CreatorId != request.CallerId)
            return Result.Failure(new Error(
                "User.NoDeletePermission",
                $"User with UserId {request.CallerId.Value} do not have permission to delete publication with PublicationId {request.PublicationId.Value}"));

        if (publication.ImageUrl != string.Empty)
            await storageService.DeleteAsync(publication.ImageUrl, cancellationToken);

        await publicationRepository.DeleteAsync(publication);

        return Result.Success();
    }
}