using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.CommentModel;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublication
{
    public class GetPublicationQueryHandler(
        IPublicationRepository publicationRepository,
        IUserRepository userRepository,
        IPublicationLikeRepositories publicationLikeRepositories,
        IFollowRepository followRepository,
        ICommentRepository commentRepository) : IQueryHandler<GetPublicationQuery, PublicationDto>
    {
        public async Task<Result<PublicationDto>> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
        {
            Publication? publication = await publicationRepository.GetByIdAsync(request.PublicationId);
            if (publication is null)
            {
                return Result.Failure<PublicationDto>(new Error(
                    "Publication.NotFound",
                    $"Publication with PublicationId {request.PublicationId.Value} was not found"));
            }

            User? user = await userRepository.GetByIdAsync(publication.CreatorId);
            if (user is null)
            {
                return Result.Failure<PublicationDto>(new Error(
                    "User.NotFound",
                    $"User with UserId {publication.CreatorId.Value} was not found"));
            }

            IReadOnlyList<PublicationLike> publicationLikes = await publicationLikeRepositories
                .GetPublicationLikesListByPublicationIdAsync(publication.Id);
            IReadOnlyList<User> followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);
            IReadOnlyList<Comment> comments = await commentRepository.GetPublicationCommentsByPublicationIdAsync(publication.Id);

            IReadOnlyList<CommentDto> commentDtos = await Task.WhenAll(comments.Where(c => c.ReplyToCommentId is not null)
                .Select(async comment =>
                {
                    IReadOnlyList<User> followers = await followRepository.GetFollowersUsersByUserIdAsync(comment.CreatorId);

                    return new CommentDto
                    {
                        Id = comment.Id,
                        Description = comment.Description,
                        CreationDateTime = comment.CreationDateTime,
                        PublicationId = comment.RelatedPublicationId,
                        CreatorInfo = new ShortUserDto
                        {
                            Id = comment.Creator!.Id,
                            FirstName = comment.Creator.FirstName,
                            LastName = comment.Creator.LastName,
                            Login = comment.Creator.Login,
                            Bio = comment.Creator.Bio,
                            ProfileImageData = comment.Creator.ProfileImageData,
                            FollowersIds = followers
                                .Select(f => f.Id)
                                .ToList() as IReadOnlyList<UserId>
                        },
                        UserWhoLikedIds = comment.CommentLikes?
                            .Select(like => like.UserId).ToList() as IReadOnlyList<UserId>
                    };
                }));

            PublicationDto publicationDto = new()
            {
                Id = publication.Id,
                Description = publication.Description,
                ImageData = publication.ImageData,
                CreationDateTime = publication.CreationDateTime,
                CreatorInfo = new ShortUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Login = user.Login,
                    Bio = user.Bio,
                    ProfileImageData = user.ProfileImageData,
                    FollowersIds = followers
                        .Select(f => f.Id)
                        .ToList() as IReadOnlyList<UserId>
                },
                UserWhoLikedIds = publicationLikes
                    .Select(like => like.UserId)
                    .ToList() as IReadOnlyList<UserId>,
                Comments = commentDtos
            };

            return Result.Success(publicationDto);
        }
    }
}
