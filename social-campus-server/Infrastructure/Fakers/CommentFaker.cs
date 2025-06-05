using Bogus;
using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers;

public static class CommentFaker
{
    public static List<Comment> Generate(int count, List<User> users, List<Publication> publications)
    {
        var faker = new Faker<Comment>()
            .CustomInstantiator(f =>
            {
                var user = f.PickRandom(users);
                var publication = f.PickRandom(publications);
                return new Comment(
                    user.Id,
                    f.Lorem.Sentences(2),
                    publication.Id);
            });

        return faker.Generate(count);
    }
}