using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler(
    IUserRepository userRepository,
    IFollowRepository followRepository
) : IQueryHandler<SearchUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<Result<IReadOnlyList<UserDto>>> Handle(SearchUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.SearchAsync(request.SearchTerm, request.Page, request.Count);

        List<UserDto> userDtos = [];
        foreach (var user in users)
        {
            var followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                ProfileImageUrl = user.ProfileImageUrl,
                FollowersIds = followers.Select(f => f.Id).ToList()
            });
        }

        return Result.Success(userDtos as IReadOnlyList<UserDto>);
    }
}