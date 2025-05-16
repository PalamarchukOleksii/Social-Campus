using Domain.Models.MessageModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasConversion(messageId => messageId.Value, value => new MessageId(value))
                .IsRequired();

            builder.Property(m => m.Text)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.SenderId)
                .IsRequired();

            builder.Property(m => m.ChatId)
                .IsRequired();

            builder.Property(m => m.SendDateTime).IsRequired();

            builder.Property(m => m.ReplyToMessageId)
                .HasConversion(id => id.Value, value => new MessageId(value));

            builder.HasIndex(m => m.Id)
                .IsUnique();

            builder.HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(m => m.ReplyToMessage)
                .WithMany(m => m.RepliedMessages)
                .HasForeignKey(m => m.ReplyToMessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);
        }
    }
}