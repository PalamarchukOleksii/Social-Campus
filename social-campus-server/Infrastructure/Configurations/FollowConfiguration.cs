using Domain.Models.FollowModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasConversion(followId => followId.Value, value => new FollowId(value))
                .IsRequired();

            builder.Property(f => f.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(f => f.FollowedUserId)
                .HasConversion(followedUserId => followedUserId.Value, value => new UserId(value))
                .IsRequired();

            builder.HasOne(f => f.User)
                .WithMany(u => u.FollowedUsers)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.FollowedUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(f => f.UserId);

            builder.HasIndex(f => f.FollowedUserId);
        }
    }
}
