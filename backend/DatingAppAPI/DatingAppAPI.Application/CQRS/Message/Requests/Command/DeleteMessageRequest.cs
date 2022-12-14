using MediatR;

namespace DatingAppAPI.Application.CQRS.Message.Requests.Command
{
    public class DeleteMessageRequest : IRequest<Unit>
    {
        public string Username { get; set; }
        public int MessageId { get; set; }
    }
}
