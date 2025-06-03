using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PublicationLikeConfiguration : IEntityTypeConfiguration<PublicationLike>
{
    public void Configure(EntityTypeBuilder<PublicationLike> builder)
    {
        builder.HasKey(pl => pl.Id);

        builder.Property(pl => pl.Id)
            .HasConversion(publicationLikeId => publicationLikeId.Value, value => new PublicationLikeId(value))
            .IsRequired();

        builder.Property(pl => pl.UserId)
            .HasConversion(userId => userId.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(pl => pl.PublicationId)
            .HasConversion(publicationId => publicationId.Value, value => new PublicationId(value))
            .IsRequired();

        builder.HasOne(pl => pl.User)
            .WithMany(u => u.PublicationLikes)
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(pl => pl.Publication)
            .WithMany(p => p.PublicationLikes)
            .HasForeignKey(pl => pl.PublicationId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasIndex(f => f.UserId);

        builder.HasIndex(f => f.PublicationId);
    }
}