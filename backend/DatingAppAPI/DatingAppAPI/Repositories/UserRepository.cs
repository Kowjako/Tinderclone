using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.DTO;
using DatingAppAPI.Interfaces;
using DatingAppAPI.Persistence.Data;
using DatingAppAPI.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<MemberDTO> GetMemberAsync(string name)
        {
            return await _dbContext.Users.Where(x => x.UserName.Equals(name))
                                         .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                         .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _dbContext.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                         .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.Include(p => p.Photos)
                                         .SingleOrDefaultAsync(x => x.UserName.Equals(username));
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dbContext.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }
    }
}
