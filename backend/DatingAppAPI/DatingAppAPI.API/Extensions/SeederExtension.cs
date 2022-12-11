using DatingAppAPI.Domain.Entities;
using DatingAppAPI.Persistence.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DatingAppAPI.Extensions
{
    public static class SeederExtension
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            // Seed Users
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            await Seeder.SeedUsers(userManager, roleManager);

            // Clear Connections on API restart
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
        }
    }
}
