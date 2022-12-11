using AutoMapper;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DatingAppAPI.Application.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _msgRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public MessageHub(IMessageRepository msgRepo, IUserRepository userRepo, IMapper mapper)
        {
            _msgRepo = msgRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            
            // For example https://localhost:5001/hubs/message?user='test'

            var otherUser = httpContext.Request.Query["user"];
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;

            var groupName = GetGroupName(username, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Send data to Angular app
            var messages = await _msgRepo.GetMessageThread(username, otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDTO dto)
        {
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;

            if (username == dto.ReceiverUsername.ToLower()) 
                throw new HubException("You cannot send messages to yourself");

            var sender = await _userRepo.GetUserByUsernameAsync(username);
            var receiver = await _userRepo.GetUserByUsernameAsync(dto.ReceiverUsername);

            if (receiver == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUsername = sender.UserName,
                ReceiverUsername = receiver.UserName,
                Content = dto.Content
            };

            _msgRepo.AddMessage(message);
            if (await _msgRepo.SaveAllAsync())
            {
                var group = GetGroupName(sender.UserName, receiver.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
