using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
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

        public async Task<PagedList<LikeDTO>> GetUserLikes(LikeParams param)
        {
            var users = _dbContext.Users.OrderBy(i => i.UserName).AsQueryable();
            var likes = _dbContext.Likes.AsQueryable();

            if(param.Predicate.Equals("liked"))
            {
                likes = likes.Where(p => p.SourceUserId == param.UserId);
                users = likes.Select(l => l.TargetUser);
            }
            else if(param.Predicate.Equals("likedBy"))
            {
                likes = likes.Where(p => p.TargetUserId == param.UserId);
                users = likes.Select(l => l.SourceUser);
            }

            /* IQueryable i projekcja poprzez Select pozwalaja nie robic Include(p => p.Photos) */
            var likedUsers = users.Select(u => new LikeDTO()
            {
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                Age = u.DateOfBirth.CalculateAge(),
                PhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = u.City,
                Id = u.Id
            });

            return await PagedList<LikeDTO>.CreateAsync(likedUsers, param.PageNumber, param.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _dbContext.Users.Include(p => p.LikedUsers)
                                         .FirstOrDefaultAsync(p => p.Id == userId);
        }
    }
}
