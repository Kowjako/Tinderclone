using DatingAppAPI.Persistence.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DatingAppAPI.Extensions
{
    public static class SeederExtension
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var scopedContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await Seeder.SeedUsers(scopedContext);
        }
    }
}
