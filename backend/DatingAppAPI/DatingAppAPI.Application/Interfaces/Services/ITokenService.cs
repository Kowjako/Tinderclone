using DatingAppAPI.Domain.Entities;

namespace DatingAppAPI.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
