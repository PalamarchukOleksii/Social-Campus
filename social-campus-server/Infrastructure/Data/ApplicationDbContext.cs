using Domain.Models.FollowModel;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
        public required DbSet<RefreshToken> RefreshTokens { get; set; }
        public required DbSet<Follow> Follows { get; set; }
        public required DbSet<Publication> Publications { get; set; }
        public required DbSet<PublicationLike> PublicationLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
