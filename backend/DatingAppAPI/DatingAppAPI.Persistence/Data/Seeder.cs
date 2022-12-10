using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatingAppAPI.Persistence.Data
{
    public class Seeder
    {
        public static async Task SeedUsers(UserManager<AppUser> mngr)
        {
            if (await mngr.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../DatingAppAPI.Persistence/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await mngr.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
