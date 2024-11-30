using Domain.Models.CommentLikeModel;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
        {
            builder.HasKey(cl => cl.Id);

            builder.Property(cl => cl.Id)
                .HasConversion(commentLikeId => commentLikeId.Value, value => new CommentLikeId(value))
                .IsRequired();

            builder.Property(cl => cl.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(cl => cl.CommentId)
                .HasConversion(commentId => commentId.Value, value => new CommentId(value))
                .IsRequired();

            builder.HasOne(cl => cl.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(cl => cl.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(cl => cl.Comment)
                .WithMany(c => c.CommentLikes)
                .HasForeignKey(cl => cl.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(f => f.UserId);
            builder.HasIndex(f => f.CommentId);
        }
    }
}
