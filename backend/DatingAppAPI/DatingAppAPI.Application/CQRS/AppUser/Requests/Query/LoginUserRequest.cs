using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Query
{
    public class LoginUserRequest : IRequest<UserDTO>
    {
        public LoginDTO LoginDTO { get; set; }
    }
}
