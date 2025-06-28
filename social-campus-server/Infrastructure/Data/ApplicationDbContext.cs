using Domain.Models.CommentLikeModel;
using Domain.Models.CommentModel;
using Domain.Models.EmailVerificationTokenModel;
using Domain.Models.FollowModel;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.PublicationTagModel;
using Domain.Models.RefreshTokenModel;
using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.TagModel;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IDataProtectionKeyContext
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Follow> Follows { get; set; }
    public required DbSet<Publication> Publications { get; set; }
    public required DbSet<PublicationLike> PublicationLikes { get; set; }
    public required DbSet<Comment> Comments { get; set; }
    public required DbSet<CommentLike> CommentLikes { get; set; }
    public required DbSet<Tag> Tags { get; set; }
    public required DbSet<PublicationTag> PublicationTags { get; set; }
    public required DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    public required DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public required DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}