using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.CommentModel;
using Domain.Shared;

namespace Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler(
        ICommentRepository commentRepository,
        IUserRepository userRepository) : ICommandHandler<UpdateCommentCommand>
    {
        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            Comment? comment = await commentRepository.GetByIdAsync(request.CommentId);
            if (comment == null)
            {
                return Result.Failure<ShortPublicationDto>(new Error(
                    "Comment.NotFound",
                    $"Comment with Comment {request.CommentId.Value} was not found"));
            }

            if (!await userRepository.IsExistByIdAsync(request.CallerId))
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"Caller with id {request.CallerId.Value} was not found"));
            }

            if (comment.CreatorId != request.CallerId)
            {
                return Result.Failure<ShortPublicationDto>(new Error(
                    "User.NoUpdatePermission",
                    $"User with UserId {request.CallerId.Value} do not have permission to update comment with CommentId {request.CommentId.Value}"));
            }

            commentRepository.Update(comment, request.Description);

            return Result.Success();
        }
    }
}
