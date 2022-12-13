using AutoMapper;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Domain.Entities;
using DatingAppAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Persistence.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public PhotoRepository(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void DeletePhoto(Photo photo)
        {
            _dbContext.Photos.Remove(photo);
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await _dbContext.Photos.IgnoreQueryFilters()
                                   .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<PhotoForApprovalDTO>> GetUnapprovedPhotos()
        {
            var list = await _dbContext.Photos.IgnoreQueryFilters()
                                              .Where(p => !p.IsApproved)
                                              .Include(p => p.AppUser)
                                              .ToListAsync();

            return _mapper.Map<List<PhotoForApprovalDTO>>(list);
        }
    }
}
