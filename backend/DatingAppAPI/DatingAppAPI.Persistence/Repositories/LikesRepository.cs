using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Domain.Entities;
using DatingAppAPI.Persistence.Data;
using DatingAppAPI.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Persistence.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _dbContext;

        public LikesRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _dbContext.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, int userId)
        {
            var users = _dbContext.Users.OrderBy(i => i.UserName).AsQueryable();
            var likes = _dbContext.Likes.AsQueryable();

            if(predicate.Equals("liked"))
            {
                likes = likes.Where(p => p.SourceUserId == userId);
                users = likes.Select(l => l.TargetUser);
            }
            else if(predicate.Equals("likedBy"))
            {
                likes = likes.Where(p => p.TargetUserId == userId);
                users = likes.Select(l => l.SourceUser);
            }

            return await users.Select(u => new LikeDTO()
            {
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                Age = u.DateOfBirth.CalculateAge(),
                PhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = u.City,
                Id = u.Id
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _dbContext.Users.Include(p => p.LikedUsers)
                                         .FirstOrDefaultAsync(p => p.Id == userId);
        }
    }
}
