using Domain.Models.ChatModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(chatId => chatId.Value, value => new ChatId(value))
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(c => c.ImageData)
                .IsRequired(false);

            builder.HasIndex(c => c.Id)
                .IsUnique();

            builder.HasMany(c => c.Participants)
                .WithOne(cp => cp.Chat)
                .HasForeignKey(cp => cp.ChatId);

            builder.HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(m => m.ChatId);
        }
    }
}