using Application.Abstractions.Security;
using Bogus;
using Domain.Models.CommentLikeModel;
using Domain.Models.CommentModel;
using Domain.Models.FollowModel;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Fakers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(DbContext context, IServiceCollection serviceCollection,
        CancellationToken ct)
    {
        if (await context.Set<User>().AsNoTracking().AnyAsync(ct)) return;

        Randomizer.Seed = new Random(42);

        var passwordHasher = serviceCollection.BuildServiceProvider().GetRequiredService<IHasher>();
        var users = await UserFaker.GenerateAsync(20, passwordHasher);
        await context.Set<User>().AddRangeAsync(users, ct);
        await context.SaveChangesAsync(ct);

        var publications = PublicationFaker.Generate(60, users);
        await context.Set<Publication>().AddRangeAsync(publications, ct);
        await context.SaveChangesAsync(ct);

        var comments = CommentFaker.Generate(150, users, publications);
        await context.Set<Comment>().AddRangeAsync(comments, ct);
        await context.SaveChangesAsync(ct);

        var commentLikes = CommentLikeFaker.Generate(users, comments);
        await context.Set<CommentLike>().AddRangeAsync(commentLikes, ct);
        await context.SaveChangesAsync(ct);

        var publicationLikes = PublicationLikeFaker.Generate(users, publications);
        await context.Set<PublicationLike>().AddRangeAsync(publicationLikes, ct);
        await context.SaveChangesAsync(ct);

        var follows = FollowFaker.Generate(users);
        await context.Set<Follow>().AddRangeAsync(follows, ct);
        await context.SaveChangesAsync(ct);
    }
}