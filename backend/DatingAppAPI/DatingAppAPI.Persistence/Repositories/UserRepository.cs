using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Domain.Entities;
using DatingAppAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Persistence.Repositories
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

        public async Task<MemberDTO> GetMemberAsync(string name, bool selfRequest)
        {            
            var query = _dbContext.Users.Where(x => x.UserName.Equals(name))
                                        .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                        .AsQueryable();

            /* Uzytkownik widzi swoje (nie cudze) niezatwierdzone zdjecia wiec wylaczymy filtr */
            if (selfRequest)
            {
                query = query.IgnoreQueryFilters();
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams param)
        {
            var query = _dbContext.Users.AsQueryable();
            query = query.Where(x => x.UserName != param.CurrentUsername)
                         .Where(x => x.Gender.Equals(param.Gender));

            var minDob = DateTime.Today.AddYears(-param.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-param.MinAge);
            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            query = param.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDTO>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDTO>(_mapper.ConfigurationProvider),
                                                          param.PageNumber, param.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByPhotoId(int photoId)
        {
            return await _dbContext.Users.Include(p => p.Photos)
                                         .IgnoreQueryFilters()
                                         .Where(p => p.Photos.Any(p => p.Id == photoId))
                                         .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.Include(p => p.Photos)
                                         .SingleOrDefaultAsync(x => x.UserName.Equals(username));
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _dbContext.Users.Where(x => x.UserName.Equals(username))
                                         .Select(x => x.Gender)
                                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dbContext.Users.Include(p => p.Photos).ToListAsync();
        }

        public void Update(AppUser user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }
    }
}
