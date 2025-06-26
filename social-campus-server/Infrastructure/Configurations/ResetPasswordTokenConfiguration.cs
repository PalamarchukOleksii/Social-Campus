using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ResetPasswordTokenConfiguration : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
    {
        builder.HasKey(vt => vt.Id);

        builder.Property(vt => vt.Id)
            .HasConversion(userId => userId.Value, value => new ResetPasswordTokenId(value))
            .IsRequired();

        builder.Property(vt => vt.UserId)
            .HasConversion(userId => userId.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(u => u.TokenHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(vt => vt.CreatedOnUtc)
            .IsRequired();

        builder.Property(vt => vt.ExpiresOnUtc)
            .IsRequired();

        builder.HasIndex(vt => vt.Id).IsUnique();
        builder.HasIndex(vt => vt.UserId);

        builder.HasOne(vt => vt.User)
            .WithMany(u => u.ResetPasswordTokens)
            .HasForeignKey(vt => vt.UserId);
    }
}