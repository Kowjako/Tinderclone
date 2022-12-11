﻿using AutoMapper;
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

            await AddToGroup(groupName);

            // Send data to Angular app
            var messages = await _msgRepo.GetMessageThread(username, otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await RemoveFromGroup();
            await base.OnDisconnectedAsync(exception);
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

            var groupName = GetGroupName(sender.UserName, receiver.UserName);
            var group = await _msgRepo.GetMessageGroup(groupName);

            if (group.Connections.Any(p => p.Username.Equals(receiver.UserName)))
            {
                message.DateRead = DateTime.UtcNow;
            }

            _msgRepo.AddMessage(message);
            if (await _msgRepo.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDTO>(message));
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
            var group = await _msgRepo.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, username);

            if(group == null)
            {
                group = new Group(groupName);
                _msgRepo.AddGroup(group);
            }

            group.Connections.Add(connection);
            return await _msgRepo.SaveAllAsync();
        }

        private async Task<bool> RemoveFromGroup()
        {
            var connection = await _msgRepo.GetConnection(Context.ConnectionId);
            _msgRepo.RemoveConnection(connection);
            return await _msgRepo.SaveAllAsync();
        }
    }
}
