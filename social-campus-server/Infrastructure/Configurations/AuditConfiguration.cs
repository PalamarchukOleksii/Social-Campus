using Domain.Enums;
using Domain.Models.AuditModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value, value => new AuditId(value))
                .IsRequired();

            builder.Property(a => a.TableName)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(a => a.FieldName)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(a => a.RecordId)
                .IsRequired();

            builder.Property(a => a.OldValue)
                .IsRequired();

            builder.Property(a => a.NewValue)
                .IsRequired();

            builder.Property(a => a.Action)
                .HasConversion(action => action.ToString(), value => Enum.Parse<AuditAction>(value))
                .IsRequired();

            builder.Property(a => a.PerformerId)
                .HasConversion(id => id.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(a => a.PerformerIp)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(a => a.PerformedAt)
                .IsRequired();

            builder.HasIndex(a => a.Id)
                .IsUnique();

            builder.HasIndex(a => a.TableName);
            builder.HasIndex(a => a.FieldName);
            builder.HasIndex(a => a.RecordId);
            builder.HasIndex(a => a.PerformerId);
            builder.HasIndex(a => a.PerformerIp);
        }
    }
}
