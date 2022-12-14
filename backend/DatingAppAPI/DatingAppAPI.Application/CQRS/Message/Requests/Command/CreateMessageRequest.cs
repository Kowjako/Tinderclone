using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Message.Requests.Command
{
    public class CreateMessageRequest : IRequest<MessageDTO>
    {
        public CreateMessageDTO CreateMessageDTO { get; set; }
        public string Username { get; set; }
    }
}
