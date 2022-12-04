using DatingAppAPI.Application.DTO;
using DatingAppAPI.Domain.Entities;

namespace DatingAppAPI.Application.Interfaces.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, int userId);
    }
}
