using Domain.Models.EmailVerificationTokenModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.HasKey(vt => vt.Id);

        builder.Property(vt => vt.Id)
            .HasConversion(userId => userId.Value, value => new EmailVerificationTokenId(value))
            .IsRequired();

        builder.Property(vt => vt.UserId)
            .HasConversion(userId => userId.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(vt => vt.CreatedOnUtc)
            .IsRequired();

        builder.Property(vt => vt.ExpiresOnUtc)
            .IsRequired();

        builder.HasIndex(vt => vt.Id).IsUnique();
        builder.HasIndex(vt => vt.UserId);

        builder.HasOne(vt => vt.User)
            .WithMany(u => u.EmailVerificationTokens)
            .HasForeignKey(vt => vt.UserId);
    }
}