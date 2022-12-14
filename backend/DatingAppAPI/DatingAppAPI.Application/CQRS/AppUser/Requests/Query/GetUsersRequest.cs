using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Query
{
    public class GetUsersRequest : IRequest<PagedList<MemberDTO>>
    {
        public UserParams Params { get; set; }
        public string Username { get; set; }
    }
}
