using DatingAppAPI.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Persistence.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions opt) : base(opt)
        {
            
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
