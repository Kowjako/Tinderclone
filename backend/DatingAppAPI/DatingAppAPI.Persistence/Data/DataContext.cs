using DatingAppAPI.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Persistance.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<AppUser> Users { get; set; }
    }
}
