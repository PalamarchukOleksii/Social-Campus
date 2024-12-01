using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Models.CommentModel;
using Domain.Shared;

namespace Application.Comments.Commands.ReplyToComment
{
    public class ReplyToCommentCommandHandler(
        IUserRepository userRepository,
        IPublicationRepository publicationRepository,
        ICommentRepository commentRepository) : ICommandHandler<ReplyToCommentCommand>
    {
        public async Task<Result> Handle(ReplyToCommentCommand request, CancellationToken cancellationToken)
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

            Comment? comment = await commentRepository.GetByIdAsync(request.ReplyToCommentId);
            if (comment is null)
            {
                return Result.Failure(new Error(
                    "Comment.NotFound",
                    $"Comment with id {request.ReplyToCommentId.Value} was not found"));
            }

            if (comment.RelatedPublicationId != request.PublicationId)
            {
                return Result.Failure(new Error(
                    "Publication.InvalidId",
                    $"PublicationId of the comment being replied to cpmment with ReplyToCommentId {request.ReplyToCommentId.Value} does not match the PublicationId in the request"));
            }

            await commentRepository.AddAsync(
                request.Description,
                request.CreatorId,
                request.PublicationId,
                request.ReplyToCommentId);

            return Result.Success();
        }
    }
}
