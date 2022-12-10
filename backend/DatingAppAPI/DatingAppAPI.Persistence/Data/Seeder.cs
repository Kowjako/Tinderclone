using DatingAppAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingAppAPI.Persistence.Data
{
    public class Seeder
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../DatingAppAPI.Persistence/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users)
            {
                user.UserName = user.UserName.ToLower();
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}
