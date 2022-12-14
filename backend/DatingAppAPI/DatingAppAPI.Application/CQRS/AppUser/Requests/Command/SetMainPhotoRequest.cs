using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Command
{
    public class SetMainPhotoRequest : IRequest<bool>
    {
        public int PhotoId { get; set; }
        public string Username { get; set; }
    }
}
