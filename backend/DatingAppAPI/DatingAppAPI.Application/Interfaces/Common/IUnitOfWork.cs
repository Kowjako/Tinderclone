using DatingAppAPI.Application.Interfaces.Repositories;

namespace DatingAppAPI.Application.Interfaces.Common
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRepository LikesRepository { get; }
        Task<bool> Complete();

        bool HasChanges();
    }
}
