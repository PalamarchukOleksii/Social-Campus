using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.UserFaker;

public class PublicationFakerFactory
{
    public static async Task<List<Publication>> GenerateAsync(int count, List<User> users)
    {
        var faker = new PublicationFaker(users);
        return faker.Generate(count);
    }
}