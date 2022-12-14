using DatingAppAPI.Application.CQRS.Message.Requests.Command;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Message.Handlers.Command
{
    public class DeleteMessageHandler : IRequestHandler<DeleteMessageRequest, Unit>
    {
        private readonly IUnitOfWork _uow;

        public DeleteMessageHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
        {
            var message = await _uow.MessageRepository.GetMessage(request.MessageId);

            if (message.ReceiverUsername.Equals(request.Username) ||
               message.SenderUsername.Equals(request.Username))
            {
                if (message.SenderUsername.Equals(request.Username)) message.SenderDeleted = true;
                if (message.ReceiverUsername.Equals(request.Username)) message.ReceiverDeleted = true;

                if (message.SenderDeleted && message.ReceiverDeleted)
                {
                    _uow.MessageRepository.DeleteMessage(message);
                }

                if (!await _uow.Complete())
                {
                    throw new HttpException(400, "Problem deleting the message");
                } 
            }
            else
            {
                throw new HttpException(401, string.Empty);
            }

            return Unit.Value;
        }
    }
}
