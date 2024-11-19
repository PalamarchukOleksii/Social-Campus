using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(172);

            builder.Property(rt => rt.TokenExpiryTime)
                .IsRequired();

            builder.HasOne(rt => rt.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rt => rt.UserId).IsUnique();

            builder.HasIndex(rt => rt.Id).IsUnique();
        }
    }
}
