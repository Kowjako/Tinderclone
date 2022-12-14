using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Query
{
    public class GetUserByIdRequest : IRequest<MemberDTO>
    {
        public string Username { get; set; }
        public bool SelfRequest { get; set; }
    }
}
