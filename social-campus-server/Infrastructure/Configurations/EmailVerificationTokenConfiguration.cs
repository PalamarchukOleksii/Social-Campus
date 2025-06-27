using Domain.Models.EmailVerificationTokenModel;
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

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.TokenHash)
            .IsRequired()
            .HasMaxLength(160);

        builder.Property(vt => vt.CreatedOnUtc)
            .IsRequired();

        builder.Property(vt => vt.ExpiresOnUtc)
            .IsRequired();

        builder.HasIndex(vt => vt.Id).IsUnique();
        builder.HasIndex(vt => vt.Email);
    }
}