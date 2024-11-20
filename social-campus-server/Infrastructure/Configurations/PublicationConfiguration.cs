using Domain.Models.PublicationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
    {
        public void Configure(EntityTypeBuilder<Publication> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => new PublicationId(value))
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.ImageData)
                .HasColumnType("varbinary(max)")
                .IsRequired(false);

            builder.Property(p => p.CreationDateTime)
                .IsRequired();

            builder.HasOne(p => p.Creator)
                .WithMany(u => u.Publications)
                .HasForeignKey(p => p.CreatorId);

            builder.HasIndex(p => p.Id)
                .IsUnique();

            builder.HasIndex(p => p.CreatorId);
        }
    }
}
