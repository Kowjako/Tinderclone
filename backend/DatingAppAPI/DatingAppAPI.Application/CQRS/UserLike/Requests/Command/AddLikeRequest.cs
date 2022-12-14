using MediatR;

namespace DatingAppAPI.Application.CQRS.UserLike.Requests.Command
{
    public class AddLikeRequest : IRequest<Unit>
    {
        public string Username { get; set; }
        public int SourceUserId { get; set; }
    }
}
