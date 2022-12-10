using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
