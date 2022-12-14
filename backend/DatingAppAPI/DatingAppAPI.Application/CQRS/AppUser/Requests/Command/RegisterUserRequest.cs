using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Command
{
    public class RegisterUserRequest : IRequest<UserDTO>
    {
        public RegisterDTO RegisterDTO { get; set; }
    }
}
