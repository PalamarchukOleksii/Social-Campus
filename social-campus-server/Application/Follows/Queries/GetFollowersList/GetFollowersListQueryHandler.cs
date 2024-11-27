using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryHandler(
        IFollowRepository followRepository,
        IUserRepository userRepository) : IQueryHandler<GetFollowersListQuery, IReadOnlyList<ShortUserDto>>
    {
        public async Task<Result<IReadOnlyList<ShortUserDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByLoginAsync(request.Login);
            if (user is null)
            {
                return Result.Failure<IReadOnlyList<ShortUserDto>>(new Error(
                    "User.NotFound",
                    $"User with login {request.Login} was not found"));
            }

            IReadOnlyList<User> response = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

            IReadOnlyList<ShortUserDto> followersDto = await Task.WhenAll(response
                .Select(async user => new ShortUserDto
                {
                    Id = user.Id,
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Bio = user.Bio,
                    ProfileImageData = user.ProfileImageData,
                    FollowersIds = (await followRepository.GetFollowersUsersByUserIdAsync(user.Id))
                        .Select(f => f.Id)
                        .ToList() as IReadOnlyList<UserId>
                }));


            return Result.Success(followersDto);
        }
    }
}
