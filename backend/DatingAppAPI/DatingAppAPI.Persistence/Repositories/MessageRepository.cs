using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Domain.Entities;
using DatingAppAPI.Persistence.Data;

namespace DatingAppAPI.Persistence.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dbContext;

        public MessageRepository(DataContext context)
        {
            _dbContext = context;
        }

        public void AddMessage(Message msg)
        {
            _dbContext.Messages.Add(msg);
        }

        public void DeleteMessage(Message msg)
        {
            _dbContext.Messages.Remove(msg);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public Task<PagedList<MessageDTO>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageDTO>> GetMessageThread(int currentUserId, int receiverId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
