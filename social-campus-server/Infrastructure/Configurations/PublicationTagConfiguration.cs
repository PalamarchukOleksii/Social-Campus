using Domain.Models.PublicationModel;
using Domain.Models.PublicationTagModel;
using Domain.Models.TagModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PublicationTagConfiguration : IEntityTypeConfiguration<PublicationTag>
{
    public void Configure(EntityTypeBuilder<PublicationTag> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .HasConversion(id => id.Value, value => new PublicationTagId(value))
            .IsRequired();

        builder.Property(pt => pt.TagId)
            .HasConversion(id => id.Value, value => new TagId(value))
            .IsRequired();

        builder.Property(pt => pt.PublicationId)
            .HasConversion(id => id.Value, value => new PublicationId(value))
            .IsRequired();

        builder.HasOne(pt => pt.Tag)
            .WithMany(t => t.PublicationTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pt => pt.Publication)
            .WithMany(p => p.PublicationTags)
            .HasForeignKey(pt => pt.PublicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pt => pt.Id).IsUnique();
        builder.HasIndex(pt => pt.TagId);
        builder.HasIndex(pt => pt.PublicationId);
    }
}