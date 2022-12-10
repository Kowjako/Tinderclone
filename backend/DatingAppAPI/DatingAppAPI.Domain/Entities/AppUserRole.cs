using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Domain.Entities
{
    // Entity for Many-to-many relationship
    // Jeden uzytkownik moze miec wiele rol, jedna rola moze nalezec do wielu uzytkownikow
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
