using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
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
                .HasConversion(refreshTokenId => refreshTokenId.Value, value => new RefreshTokenId(value))
                .IsRequired();

            builder.Property(rt => rt.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(172);

            builder.Property(rt => rt.TokenExpiryTime)
                .IsRequired();

            builder.HasOne(rt => rt.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(rt => rt.UserId);

            builder.HasIndex(rt => rt.UserId).IsUnique();

            builder.HasIndex(rt => rt.Id).IsUnique();
        }
    }
}
