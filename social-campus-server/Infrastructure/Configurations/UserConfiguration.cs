using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(50);

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

            builder.HasIndex(u => u.Id)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.Login)
                .IsUnique();

            builder.HasOne(u => u.RefreshToken)
                .WithOne(rt => rt.User)
                .HasForeignKey<RefreshToken>(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
