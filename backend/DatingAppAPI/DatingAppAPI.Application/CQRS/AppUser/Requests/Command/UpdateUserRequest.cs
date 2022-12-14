using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Command
{
    public class UpdateUserRequest : IRequest<Unit>
    {
        public string Username { get; set; }
        public MemberUpdateDTO MemberUpdateDTO { get; set; }
    }
}
