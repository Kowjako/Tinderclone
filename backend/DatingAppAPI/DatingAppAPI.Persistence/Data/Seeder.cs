using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace DatingAppAPI.Persistence.Data
{
    public class Seeder
    {
        public static async Task SeedUsers(UserManager<AppUser> mngr, RoleManager<AppRole> roleMngr)
        {
            if (await mngr.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../DatingAppAPI.Persistence/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole { Name = "Member" },
                new AppRole { Name = "Admin" },
                new AppRole { Name = "Moderator" },
            };

            foreach (var role in roles)
            {
                await roleMngr.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.Photos.First().IsApproved = true;

                await mngr.CreateAsync(user, "Pa$$w0rd");
                await mngr.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "Admin",
            };

            admin.City = admin.Country = admin.Gender = admin.LookingFor =
            admin.KnownAs = admin.Introduction = admin.Interests = string.Empty;

            await mngr.CreateAsync(admin, "Pa$$w0rd");
            await mngr.AddToRolesAsync(admin, new[] { "Member", "Admin" });
        }
    }
}
