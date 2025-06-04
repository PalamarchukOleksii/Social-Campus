using Application.Abstractions.Security;
using Domain.Models.UserModel;

namespace Infrastructure.Fakers.UserFaker;

public static class UserFakerFactory
{
    public static async Task<List<User>> GenerateAsync(int count, IPasswordHasher passwordHasher)
    {
        var faker = new UserFaker();
        var users = faker.Generate(count);

        foreach (var user in users)
        {
            var hashedPassword = await passwordHasher.HashAsync(user.PasswordHash);
            user.UpdatePasswordHash(hashedPassword);
        }

        return users;
    }
}