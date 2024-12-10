using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Comments.Queries.GetRepliesToComment
{
    public class GetRepliesToCommentQueryHandler(
        ICommentRepository commentRepository,
        IFollowRepository followRepository) : IQueryHandler<GetRepliesToCommentQuery, IReadOnlyList<CommentDto>>
    {
        public async Task<Result<IReadOnlyList<CommentDto>>> Handle(GetRepliesToCommentQuery request, CancellationToken cancellationToken)
        {
            bool isCommentExist = await commentRepository.IsExistByIdAsync(request.CommentId);
            if (!isCommentExist)
            {
                return Result.Failure<IReadOnlyList<CommentDto>>(new Error(
                    "Comment.NotFound",
                    $"Comment with id {request.CommentId.Value} was not found"));
            }

            IReadOnlyList<Comment> comments = await commentRepository.GetRepliedCommentsByCommentIdAsync(request.CommentId);

            IReadOnlyList<CommentDto> commentDtos = await Task.WhenAll(comments.Select(async c =>
            {
                IReadOnlyList<User> followers = await followRepository.GetFollowersUsersByUserIdAsync(c.CreatorId);

                return new CommentDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreationDateTime = c.CreationDateTime,
                    PublicationId = c.RelatedPublicationId,
                    CreatorInfo = new ShortUserDto
                    {
                        Id = c.Creator!.Id,
                        FirstName = c.Creator.FirstName,
                        LastName = c.Creator.LastName,
                        Login = c.Creator.Login,
                        Bio = c.Creator.Bio,
                        ProfileImageData = c.Creator.ProfileImageData,
                        FollowersIds = followers.Select(f => f.Id).ToList() as IReadOnlyList<UserId>
                    },
                    UserWhoLikedIds = c.CommentLikes?
                        .Select(like => like.UserId).ToList() as IReadOnlyList<UserId>
                };
            }));

            return Result.Success(commentDtos.ToList() as IReadOnlyList<CommentDto>);

        }
    }
}
