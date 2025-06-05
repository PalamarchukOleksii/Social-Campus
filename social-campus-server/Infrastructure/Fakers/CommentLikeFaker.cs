using Bogus;
using Domain.Models.CommentLikeModel;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers;

public static class CommentLikeFaker
{
    public static List<CommentLike> Generate(List<User> users, List<Comment> comments)
    {
        var faker = new Faker();
        return comments
            .SelectMany(comment =>
            {
                var likesCount = faker.Random.Int(0, 5);
                var likingUsers = faker.PickRandom(users, likesCount);
                return likingUsers.Select(user => new CommentLike(user.Id, comment.Id));
            })
            .ToList();
    }
}