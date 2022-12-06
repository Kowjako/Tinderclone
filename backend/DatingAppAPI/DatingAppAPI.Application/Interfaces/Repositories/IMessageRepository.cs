using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Domain.Entities;

namespace DatingAppAPI.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message msg);
        void DeleteMessage(Message msg);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams param);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string username, string receiverName);
        Task<bool> SaveAllAsync();
    }
}
