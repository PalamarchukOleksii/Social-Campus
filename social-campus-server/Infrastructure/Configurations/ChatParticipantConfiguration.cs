using Domain.Models.ChatModel;
using Domain.Models.ChatParticipantModel;
using Domain.Models.ChatParticipantRoleModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
    {
        public void Configure(EntityTypeBuilder<ChatParticipant> builder)
        {
            builder.HasKey(cp => cp.Id);

            builder.Property(cp => cp.Id)
                .HasConversion(chatParticipantId => chatParticipantId.Value, value => new ChatParticipantId(value))
                .IsRequired();

            builder.Property(cp => cp.ChatId)
                .HasConversion(chatId => chatId.Value, value => new ChatId(value))
                .IsRequired();

            builder.Property(cp => cp.UserId)
                .HasConversion(userId => userId.Value, value => new UserId(value))
                .IsRequired();
            builder.Property(cp => cp.RoleId)
                .HasConversion(roleId => roleId.Value, value => new ChatParticipantRoleId(value))
                .IsRequired();

            builder.HasIndex(cp => cp.Id)
                .IsUnique();

            builder.HasOne(cp => cp.Chat)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.ChatId);

            builder.HasOne(cp => cp.User)
                .WithMany(u => u.ChatParticipations)
                .HasForeignKey(cp => cp.UserId);

            builder.HasOne(cp => cp.Role)
                .WithMany(r => r.ChatParticipants)
                .HasForeignKey(cp => cp.RoleId);
        }
    }
}