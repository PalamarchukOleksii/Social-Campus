using Application.Abstractions.Security;
using Bogus;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers;

public static class UserFaker
{
    public static async Task<List<User>> GenerateAsync(int count, IPasswordHasher passwordHasher)
    {
        var faker = new Faker<User>();

        faker.CustomInstantiator(f => new User(
            f.Internet.UserName(),
            f.Internet.Password(),
            f.Internet.Email(),
            f.Name.FirstName(),
            f.Name.LastName()));

        faker.RuleFor(u => u.Bio, f => f.Lorem.Sentence());
        faker.RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar());

        var users = faker.Generate(count);

        foreach (var user in users)
        {
            var hashedPassword = await passwordHasher.HashAsync(user.PasswordHash);
            user.UpdatePasswordHash(hashedPassword);
        }

        return users;
    }
}