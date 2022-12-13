using AutoMapper;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Persistence.Data;

namespace DatingAppAPI.Persistence.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_context);

        public IPhotoRepository PhotoRepository => new PhotoRepository(_context, _mapper);

        public async Task<bool> Complete() => await _context.SaveChangesAsync() > 0;

        public bool HasChanges() => _context.ChangeTracker.HasChanges();
    }
}
