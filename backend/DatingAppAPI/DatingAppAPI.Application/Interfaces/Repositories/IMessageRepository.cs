﻿using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Domain.Entities;

namespace DatingAppAPI.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message msg);
        void DeleteMessage(Message msg);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUser();
        Task<IEnumerable<MessageDTO>> GetMessageThread(int currentUserId, int receiverId);
        Task<bool> SaveAllAsync();
    }
}