using DatingAppAPI.Domain.Entities;

namespace DatingAppAPI.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
