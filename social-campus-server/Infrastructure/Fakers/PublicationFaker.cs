using Bogus;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers;

public static class PublicationFaker
{
    public static List<Publication> Generate(int count, List<User> users)
    {
        var faker = new Faker<Publication>()
            .CustomInstantiator(f =>
            {
                var user = f.PickRandom(users);
                var hasImage = f.Random.Bool(0.5f);
                var imageUrl = hasImage ? f.Image.PicsumUrl() : string.Empty;

                return new Publication(
                    f.Lorem.Paragraph(),
                    user.Id,
                    imageUrl
                );
            });

        return faker.Generate(count);
    }
}