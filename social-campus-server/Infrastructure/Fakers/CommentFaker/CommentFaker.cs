using Bogus;
using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.CommentFaker;

public class CommentFaker : Faker<Comment>
{
    public CommentFaker(List<User> users, List<Publication> publications)
    {
        CustomInstantiator(f =>
        {
            var user = f.PickRandom(users);
            var publication = f.PickRandom(publications);
            return new Comment(
                user.Id,
                f.Lorem.Sentences(2),
                publication.Id);
        });
    }
}