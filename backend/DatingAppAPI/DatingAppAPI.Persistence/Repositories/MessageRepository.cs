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
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
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

        public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams param)
        {
            var query = _dbContext.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

            query = param.Container switch
            {
                "Inbox" => query.Where(p => p.ReceiverUsername.Equals(param.Username)),
                "Outbox" => query.Where(p => p.SenderUsername.Equals(param.Username)),
                _ => query.Where(p => p.ReceiverUsername.Equals(param.Username) && p.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDTO>.CreateAsync(messages, param.PageNumber, param.PageSize);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string senderName, string receiverName)
        {
            var messages = await _dbContext.Messages.Include(p => p.Sender).ThenInclude(p => p.Photos)
                                                    .Include(p => p.Receiver).ThenInclude(p => p.Photos)
                                                    .Where(m => m.ReceiverUsername == senderName &&
                                                                m.SenderUsername == receiverName ||
                                                                m.ReceiverUsername == receiverName &&
                                                                m.SenderUsername == senderName)
                                                    .OrderByDescending(m => m.MessageSent)
                                                    .ToListAsync();

            var unreadMsg = messages.Where(p => p.DateRead == null && p.ReceiverUsername == senderName).ToList();

            if(unreadMsg.Any())
            {
                unreadMsg.ForEach(msg => msg.DateRead = DateTime.UtcNow);
                await _dbContext.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
