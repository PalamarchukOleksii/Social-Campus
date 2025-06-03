using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(userId => userId.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(u => u.RefreshTokenId)
            .HasConversion(refreshTokenId => refreshTokenId.Value, value => new RefreshTokenId(value));

        builder.Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Bio)
            .HasMaxLength(500);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.ProfileImageData)
            .IsRequired(false);

        builder.HasIndex(u => u.Id)
            .IsUnique();

        builder.HasIndex(u => u.RefreshTokenId);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Login)
            .IsUnique();

        builder.HasOne(u => u.RefreshToken)
            .WithOne(rt => rt.User)
            .HasForeignKey<RefreshToken>(rt => rt.UserId);

        builder.HasMany(u => u.FollowedUsers)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserId);

        builder.HasMany(u => u.Followers)
            .WithOne(f => f.FollowedUser)
            .HasForeignKey(f => f.FollowedUserId);

        builder.HasMany(u => u.Publications)
            .WithOne(p => p.Creator)
            .HasForeignKey(p => p.CreatorId);
    }
}