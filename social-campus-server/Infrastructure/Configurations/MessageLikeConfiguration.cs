using Domain.Models.MessageLikeModel;
using Domain.Models.MessageModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class MessageLikeConfiguration : IEntityTypeConfiguration<MessageLike>
    {
        public void Configure(EntityTypeBuilder<MessageLike> builder)
        {
            builder.HasKey(ml => ml.Id);

            builder.Property(ml => ml.Id)
                .HasConversion(messageLikeId => messageLikeId.Value, value => new MessageLikeId(value))
                .IsRequired();

            builder.Property(ml => ml.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(ml => ml.MessageId)
                .HasConversion(messageId => messageId.Value, value => new MessageId(value))
                .IsRequired();

            builder.HasIndex(ml => ml.Id)
                .IsUnique();

            builder.HasOne(ml => ml.User)
                .WithMany(u => u.MessageLikes)
                .HasForeignKey(ml => ml.UserId);

            builder.HasOne(ml => ml.Message)
                .WithMany(m => m.MessageLikes)
                .HasForeignKey(ml => ml.MessageId);
        }
    }
}