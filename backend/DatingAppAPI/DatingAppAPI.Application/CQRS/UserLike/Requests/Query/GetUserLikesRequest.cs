using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;

namespace DatingAppAPI.Application.CQRS.UserLike.Requests.Query
{
    public class GetUserLikesRequest : IRequest<PagedList<LikeDTO>>
    {
        public LikeParams Params { get; set; }
        public string UserId { get; set; }
    }
}
