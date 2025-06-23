using Domain.Models.TagModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new TagId(value))
            .IsRequired();

        builder.Property(p => p.Label)
            .IsRequired();

        builder.HasIndex(p => p.Id)
            .IsUnique();

        builder.HasIndex(p => p.Label)
            .IsUnique();

        builder.HasMany(t => t.PublicationTags)
            .WithOne(pt => pt.Tag)
            .HasForeignKey(pt => pt.TagId);
    }
}