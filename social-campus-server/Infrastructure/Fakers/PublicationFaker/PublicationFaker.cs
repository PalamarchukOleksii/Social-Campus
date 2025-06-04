using Bogus;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.PublicationFaker;

public class PublicationFaker : Faker<Publication>
{
    public PublicationFaker(List<User> users)
    {
        CustomInstantiator(f =>
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
    }
}