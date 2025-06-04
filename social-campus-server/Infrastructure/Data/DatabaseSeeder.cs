using Application.Abstractions.Security;
using Bogus;
using Domain.Models.CommentLikeModel;
using Domain.Models.CommentModel;
using Domain.Models.FollowModel;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Fakers.CommentFaker;
using Infrastructure.Fakers.CommentLikeFaker;
using Infrastructure.Fakers.FollowFaker;
using Infrastructure.Fakers.PublicationFaker;
using Infrastructure.Fakers.PublicationLikeFaker;
using Infrastructure.Fakers.UserFaker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider,
        CancellationToken ct)
    {
        if (await context.Set<User>().AsNoTracking().AnyAsync(ct)) return;

        Randomizer.Seed = new Random(42);

        var passwordHasher = serviceProvider.GetService<IPasswordHasher>()
                             ?? throw new InvalidOperationException("IPasswordHasher not registered");

        var users = await UserFakerFactory.GenerateAsync(20, passwordHasher);
        await context.Set<User>().AddRangeAsync(users, ct);
        await context.SaveChangesAsync(ct);

        var publications = PublicationFakerFactory.Generate(60, users);
        await context.Set<Publication>().AddRangeAsync(publications, ct);
        await context.SaveChangesAsync(ct);

        var comments = CommentFakerFactory.Generate(150, users, publications);
        await context.Set<Comment>().AddRangeAsync(comments, ct);
        await context.SaveChangesAsync(ct);

        var commentLikes = CommentLikeFakerFactory.Generate(users, comments);
        await context.Set<CommentLike>().AddRangeAsync(commentLikes, ct);
        await context.SaveChangesAsync(ct);

        var publicationLikes = PublicationLikeFakerFactory.Generate(users, publications);
        await context.Set<PublicationLike>().AddRangeAsync(publicationLikes, ct);
        await context.SaveChangesAsync(ct);

        var follows = FollowFakerFactory.Generate(users);
        await context.Set<Follow>().AddRangeAsync(follows, ct);
        await context.SaveChangesAsync(ct);
    }
}