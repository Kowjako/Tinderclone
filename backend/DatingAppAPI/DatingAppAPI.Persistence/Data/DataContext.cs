using DatingAppAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Persistence.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions opt) : base(opt)
        {
            
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                   .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            builder.Entity<UserLike>()
                   .HasOne(s => s.SourceUser)
                   .WithMany(l => l.LikedUsers)
                   .HasForeignKey(l => l.SourceUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                   .HasOne(s => s.TargetUser)
                   .WithMany(l => l.LikedByUsers)
                   .HasForeignKey(l => l.TargetUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
