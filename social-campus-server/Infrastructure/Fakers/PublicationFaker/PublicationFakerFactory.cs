using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.PublicationFaker;

public static class PublicationFakerFactory
{
    public static List<Publication> Generate(int count, List<User> users)
    {
        var faker = new PublicationFaker(users);
        return faker.Generate(count);
    }
}