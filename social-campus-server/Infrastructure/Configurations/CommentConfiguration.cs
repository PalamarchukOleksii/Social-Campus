using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(id => id.Value, value => new CommentId(value))
                .IsRequired();

            builder.Property(c => c.RelatedPublicationId)
                .HasConversion(relPubId => relPubId.Value, value => new PublicationId(value))
                .IsRequired();

            builder.Property(c => c.ReplyToCommentId)
                .HasConversion(
                    relComId => relComId.Value != Guid.Empty ? relComId.Value : null as Guid?,
                    value => value.HasValue ? new CommentId(value.Value) : new CommentId(Guid.Empty)
                )
                .IsRequired(false);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.CreationDateTime)
                .IsRequired();

            builder.HasOne(c => c.Creator)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.RelatedPublication)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.RelatedPublicationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.ReplyToComment)
                .WithMany(c => c.RepliedComments)
                .HasForeignKey(c => c.ReplyToCommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            builder.HasIndex(p => p.Id).IsUnique();
            builder.HasIndex(p => p.CreatorId);
            builder.HasIndex(p => p.RelatedPublicationId);
            builder.HasIndex(p => p.ReplyToCommentId);
        }
    }
}
