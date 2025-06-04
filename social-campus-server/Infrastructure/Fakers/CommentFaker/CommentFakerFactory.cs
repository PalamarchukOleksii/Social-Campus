using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.CommentFaker;

public static class CommentFakerFactory
{
    public static List<Comment> Generate(int count, List<User> users, List<Publication> publications)
    {
        var faker = new CommentFaker(users, publications);
        return faker.Generate(count);
    }
}