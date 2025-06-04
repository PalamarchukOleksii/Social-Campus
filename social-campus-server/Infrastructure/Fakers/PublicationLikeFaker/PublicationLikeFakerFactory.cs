using Bogus;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.PublicationLikeFaker;

public static class PublicationLikeFakerFactory
{
    public static List<PublicationLike> Generate(List<User> users, List<Publication> publications)
    {
        var faker = new Faker();
        return publications
            .SelectMany(comment =>
            {
                var likesCount = faker.Random.Int(0, 5);
                var likingUsers = faker.PickRandom(users, likesCount);
                return likingUsers.Select(user => new PublicationLike(user.Id, comment.Id));
            })
            .ToList();
    }
}