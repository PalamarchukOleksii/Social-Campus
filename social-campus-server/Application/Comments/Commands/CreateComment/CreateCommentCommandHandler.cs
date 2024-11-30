using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        IPublicationRepository publicationRepository,
        IUserRepository userRepository) : ICommandHandler<CreateCommentCommand>
    {
        public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            bool isUserExist = await userRepository.IsExistByIdAsync(request.CreatorId);
            if (!isUserExist)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with id {request.CreatorId.Value} was not found"));
            }

            bool isPublicationExist = await publicationRepository.IsExistByIdAsync(request.PublicationId);
            if (!isPublicationExist)
            {
                return Result.Failure(new Error(
                    "Publication.NotFound",
                    $"Publication with id {request.PublicationId.Value} was not found"));
            }

            await commentRepository.AddAsync(request.Description, request.CreatorId, request.PublicationId);

            return Result.Success();
        }
    }
}
