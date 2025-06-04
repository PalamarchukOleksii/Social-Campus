using Bogus;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.UserFaker;

public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        CustomInstantiator(f => new User(
            f.Internet.UserName(),
            f.Internet.Password(),
            f.Internet.Email(),
            f.Name.FirstName(),
            f.Name.LastName()));

        RuleFor(u => u.Bio, f => f.Lorem.Sentence());
        RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar());
    }
}