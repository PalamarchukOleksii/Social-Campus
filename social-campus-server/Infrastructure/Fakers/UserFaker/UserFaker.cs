using Application.Abstractions.Security;
using Bogus;

namespace Infrastructure.Fakers.User;

public class UserFaker : Faker<User>
{
    public UserFaker(IPasswordHasher passwordHasher)
    {
        CustomInstantiator(f => new User(
            f.Internet.UserName(),
            await passwordHasher.HashAsync(f.Internet.Password()),
            f.Internet.Email(),
            f.Name.FirstName(),
            f.Name.LastName()));

        RuleFor(u => u.Bio, f => f.Lorem.Sentence());
        RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar());
    }
}