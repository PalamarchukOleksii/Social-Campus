using System;
using Domain.Enums;
using Domain.Models.UserModel;
using Domain.Models.UserRestrictionModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserrestrictionConfiguration : IEntityTypeConfiguration<UserRestriction>
    {
        public void Configure(EntityTypeBuilder<UserRestriction> builder)
        {
            builder.HasKey(ur => ur.Id);

            builder.Property(ur => ur.Id)
                .HasConversion(urId => urId.Value, value => new UserRestrictionId(value))
                .IsRequired();

            builder.Property(ur => ur.TargetUserId)
                .HasConversion(urId => urId.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(ur => ur.ImposedByUserId)
                .HasConversion(urId => urId.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(ur => ur.Type)
                .HasConversion(
                    urType => urType.ToString(),
                    value => Enum.Parse<RestrictionType>(value))
                .IsRequired();

            builder.Property(ur => ur.Reason)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasIndex(ur => ur.TargetUserId)
                .IsUnique();

            builder.Property(ur => ur.StartAt).IsRequired();

            builder.Property(ur => ur.EndAt)
                .IsRequired(false);

            builder.HasIndex(ur => ur.ImposedByUserId);
            builder.HasIndex(ur => ur.Type);

            builder.HasOne(ur => ur.TargetUser)
                .WithMany()
                .HasForeignKey(ur => ur.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ur => ur.ImposedByUser)
                .WithMany()
                .HasForeignKey(ur => ur.ImposedByUserId);
        }
    }
}
