using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Requests.Command
{
    public class RejectPhotoRequest : IRequest<Unit>
    {
        public int PhotoId { get; set; }
    }
}
