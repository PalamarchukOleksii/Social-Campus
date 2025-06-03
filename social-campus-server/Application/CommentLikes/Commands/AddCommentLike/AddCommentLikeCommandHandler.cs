using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.CommentLikes.Commands.AddCommentLike;

public class AddCommentLikeCommandHandler(
    IUserRepository userRepository,
    ICommentLikeRepository commentLikeRepository,
    ICommentRepository commentRepository) : ICommandHandler<AddCommentLikeCommand>
{
    public async Task<Result> Handle(AddCommentLikeCommand request, CancellationToken cancellationToken)
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
        if (isAlreadiAddLike)
            return Result.Failure(new Error(
                "CommentLike.AlreadyAddLike",
                $"User with UserId {request.UserId.Value} already add like to comment with CommentId {request.CommentId.Value}"));

        await commentLikeRepository.AddAsync(request.UserId, request.CommentId);

        return Result.Success();
    }
}