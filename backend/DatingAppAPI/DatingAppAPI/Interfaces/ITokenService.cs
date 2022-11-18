using DatingAppAPI.Persistance.Entities;

namespace DatingAppAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
