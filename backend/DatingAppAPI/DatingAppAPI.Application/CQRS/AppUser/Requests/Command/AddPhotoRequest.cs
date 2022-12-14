using DatingAppAPI.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DatingAppAPI.Application.CQRS.AppUser.Requests.Command
{
    public class AddPhotoRequest : IRequest<PhotoDTO>
    {
        public IFormFile File { get; set; }
        public string Username { get; set; }
    }
}
