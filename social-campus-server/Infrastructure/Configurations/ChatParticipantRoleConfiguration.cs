using Domain.Models.ChatParticipantRoleModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ChatParticipantRoleConfiguration : IEntityTypeConfiguration<ChatParticipantRole>
    {
        public void Configure(EntityTypeBuilder<ChatParticipantRole> builder)
        {
            builder.HasKey(cpr => cpr.Id);

            builder.Property(cpr => cpr.Id)
                .HasConversion(roleId => roleId.Value, value => new ChatParticipantRoleId(value))
                .IsRequired();

            builder.Property(cpr => cpr.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(cpr => cpr.Id)
                .IsUnique();

            builder.HasMany(cpr => cpr.ChatParticipants)
                .WithOne(cp => cp.Role)
                .HasForeignKey(cp => cp.RoleId);
        }
    }
}