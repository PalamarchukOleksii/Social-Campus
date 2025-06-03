using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.PublicationLikes.Commands.RemovePublicationLike;

public class RemovePublicationLikeCommandHandler(
    IUserRepository userRepository,
    IPublicationRepository publicationRepository,
    IPublicationLikeRepositories publicationLikeRepositories) : ICommandHandler<RemovePublicationLikeCommand>
{
    public async Task<Result> Handle(RemovePublicationLikeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with UserId {request.UserId.Value} was not found"));

        var publication = await publicationRepository.GetByIdAsync(request.PublicationId);
        if (publication is null)
            return Result.Failure(new Error(
                "Publication.NotFound",
                $"Publication with PublicationId {request.PublicationId.Value} was not found"));

        var isAlreadiAddLike = await publicationLikeRepositories.IsLike(request.UserId, request.PublicationId);
        if (!isAlreadiAddLike)
            return Result.Failure(new Error(
                "PublicationLike.NotAddLikeYet",
                $"User with UserId {request.UserId.Value} was not add like to publication with PublicationId {request.PublicationId.Value}"));

        await publicationLikeRepositories.DeleteAsync(request.UserId, request.PublicationId);

        return Result.Success();
    }
}