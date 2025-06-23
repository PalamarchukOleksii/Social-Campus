using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
    : ICommandHandler<DeleteCommentCommand>
{
    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.IsExistByIdAsync(request.CallerId))
            return Result.Failure(new Error(
                "User.NotFound",
                $"Caller with id {request.CallerId.Value} was not found"));

        var comment = await commentRepository.GetByIdAsync(request.CommentId);
        if (comment is null)
            return Result.Failure(new Error(
                "Comment.NotFound",
                $"Comment with CommentId {request.CommentId.Value} was not found"));

        if (comment.CreatorId != request.CallerId)
            return Result.Failure(new Error(
                "User.NoDeletePermission",
                $"User with UserId {request.CallerId.Value} do not have permission to delete comment with CommentId {request.CommentId.Value}"));

        commentRepository.Delete(comment);

        return Result.Success();
    }
}