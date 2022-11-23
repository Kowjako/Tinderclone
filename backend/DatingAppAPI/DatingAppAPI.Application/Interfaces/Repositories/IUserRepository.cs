using DatingAppAPI.Application.DTO;
using DatingAppAPI.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingAppAPI.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<MemberDTO> GetMemberAsync(string name);
    }
}
