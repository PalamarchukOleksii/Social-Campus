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

                return new Publication(
                    f.Lorem.Paragraph(),
                    user.Id,
                    ""
                );
            });

        return faker.Generate(count);
    }
}