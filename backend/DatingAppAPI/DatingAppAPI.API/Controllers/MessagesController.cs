using AutoMapper;
using DatingAppAPI.API.Extensions;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Controllers;
using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public MessagesController(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage([FromBody] CreateMessageDTO dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (username == dto.ReceiverUsername.ToLower()) return BadRequest("You cannot send message to yourself");

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var receiver = await _uow.UserRepository.GetUserByUsernameAsync(dto.ReceiverUsername);

            if (receiver == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUsername = sender.UserName,
                ReceiverUsername = receiver.UserName,
                Content = dto.Content
            };

            _uow.MessageRepository.AddMessage(message);
            if (!await _uow.Complete()) return BadRequest("Failed to send message");

            return Ok(_mapper.Map<MessageDTO>(message));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams param)
        {
            param.Username = User.FindFirst(ClaimTypes.Name)?.Value;
            var msg = await _uow.MessageRepository.GetMessagesForUser(param);

            Response.AddPaginationHeader(new PaginationHeader(msg.CurrentPage,
                                                              msg.PageSize,
                                                              msg.TotalCount,
                                                              msg.TotalPages));
            return Ok(msg);
        }

        [HttpDelete("{msgId}")]
        public async Task<ActionResult> DeleteMessage(int msgId)
        {
            var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
            var message = await _uow.MessageRepository.GetMessage(msgId);

            if(message.ReceiverUsername.Equals(currentUserName) ||
               message.SenderUsername.Equals(currentUserName))
            {
                if (message.SenderUsername.Equals(currentUserName)) message.SenderDeleted = true;
                if (message.ReceiverUsername.Equals(currentUserName)) message.ReceiverDeleted = true;

                if(message.SenderDeleted && message.ReceiverDeleted)
                {
                    _uow.MessageRepository.DeleteMessage(message);
                }

                if(await _uow.Complete())
                {
                    return Ok();
                }
                return BadRequest("Problem deleting the message");
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
