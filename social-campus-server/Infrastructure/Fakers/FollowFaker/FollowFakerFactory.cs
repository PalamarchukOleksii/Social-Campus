using Bogus;
using Domain.Models.FollowModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.FollowFaker;

public static class FollowFakerFactory
{
    public static List<Follow> Generate(List<User> users)
    {
        var faker = new Faker();
        return users
            .SelectMany(user =>
            {
                var followTargets = users
                    .Where(u => u.Id != user.Id)
                    .OrderBy(_ => faker.Random.Int())
                    .Take(faker.Random.Int(0, 10));
                return followTargets.Select(target => new Follow(user.Id, target.Id));
            })
            .ToList();
    }
}