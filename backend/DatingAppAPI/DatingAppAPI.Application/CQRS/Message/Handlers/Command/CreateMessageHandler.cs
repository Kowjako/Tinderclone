using AutoMapper;
using DatingAppAPI.Application.CQRS.Message.Requests.Command;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;
using Msg = DatingAppAPI.Domain.Entities.Message;

namespace DatingAppAPI.Application.CQRS.Message.Handlers.Command
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageRequest, MessageDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateMessageHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<MessageDTO> Handle(CreateMessageRequest request, CancellationToken cancellationToken)
        {
            if (request.Username == request.CreateMessageDTO.ReceiverUsername.ToLower())
                throw new HttpException(400, "You cannot send message to yourself");

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(request.Username);
            var receiver = await _uow.UserRepository.GetUserByUsernameAsync(request.CreateMessageDTO.ReceiverUsername);

            if (receiver == null) throw new HttpException(404, string.Empty);

            var message = new Msg
            {
                Sender = sender,
                Receiver = receiver,
                SenderUsername = sender.UserName,
                ReceiverUsername = receiver.UserName,
                Content = request.CreateMessageDTO.Content
            };

            _uow.MessageRepository.AddMessage(message);
            if (!await _uow.Complete())
                throw new HttpException(400, "Failed to send message");

            return _mapper.Map<MessageDTO>(message);
        }
    }
}
