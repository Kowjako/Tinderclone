using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Requests.Command
{
    public class ApprovePhotoRequest : IRequest<PhotoDTO>
    {
        public int PhotoId { get; set; }
    }
}
