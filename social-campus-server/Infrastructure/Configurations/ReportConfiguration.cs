using Domain.Models.ReportModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasConversion(rid => rid.Value, value => new ReportId(value))
                .IsRequired();

            builder.Property(r => r.ReporterId)
                .HasConversion(uid => uid.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(r => r.TargetType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.TargetId)
                .IsRequired();

            builder.Property(r => r.Reason)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.Resolved)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(r => r.ResolvedById)
                .HasConversion(rid => rid.Value, value => new UserId(value))
                .IsRequired(false);

            builder.Property(r => r.ResolutionComment)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(r => r.ResolvedAt)
                .IsRequired(false);

            builder.HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReporterId);

            builder.HasOne(r => r.Resolver)
                .WithMany()
                .HasForeignKey(r => r.ResolvedById);

            builder.HasIndex(r => r.Id)
                .IsUnique();

            builder.HasIndex(r => r.ReporterId);
            builder.HasIndex(r => r.TargetId);
            builder.HasIndex(r => r.TargetType);
            builder.HasIndex(r => r.ResolvedById);
        }
    }
}
