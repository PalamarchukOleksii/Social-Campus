using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.CommentLikes.Commands.RemoveCommentLike;

public class RemoveCommentLikeCommandHandler(
    IUserRepository userRepository,
    ICommentLikeRepository commentLikeRepository,
    ICommentRepository commentRepository) : ICommandHandler<RemoveCommentLikeCommand>
{
    public async Task<Result> Handle(RemoveCommentLikeCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = await userRepository.IsExistByIdAsync(request.UserId);
        if (!isUserExist)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with UserId {request.UserId.Value} was not found"));

        var isCommentExist = await commentRepository.IsExistByIdAsync(request.CommentId);
        if (!isCommentExist)
            return Result.Failure(new Error(
                "Comment.NotFound",
                $"Comment with PublicationId {request.CommentId.Value} was not found"));

        var isAlreadiAddLike = await commentLikeRepository.IsLike(request.UserId, request.CommentId);
        if (!isAlreadiAddLike)
            return Result.Failure(new Error(
                "CommentLike.NotAddLikeYet",
                $"User with UserId {request.UserId.Value} was not add like to comment with CommentId {request.CommentId.Value}"));

        await commentLikeRepository.DeleteAsync(request.UserId, request.CommentId);

        return Result.Success();
    }
}