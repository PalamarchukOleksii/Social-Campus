using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Commands.UpdatePublication;

public class UpdatePublicationCommandHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository) : ICommandHandler<UpdatePublicationCommand>
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

        publicationRepository.Update(publication, request.Description, request.ImageData);

        return Result.Success();
    }
}