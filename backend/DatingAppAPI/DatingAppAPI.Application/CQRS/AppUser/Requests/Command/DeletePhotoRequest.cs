using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Command
{
    public class DeletePhotoRequest : IRequest<Unit>
    {
        public string Username { get; set; }
        public int PhotoId { get; set; }
    }
}
