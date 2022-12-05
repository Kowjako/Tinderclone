using AutoMapper;
using DatingAppAPI.API.Extensions;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Application.Interfaces.Repositories;
using DatingAppAPI.Controllers;
using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepo;
        private readonly IMessageRepository _msgRepo;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepo, IMessageRepository msgRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _msgRepo = msgRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage([FromBody] CreateMessageDTO dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (username == dto.ReceiverUsername.ToLower()) return BadRequest("You cannot send message to yourself");

            var sender = await _userRepo.GetUserByUsernameAsync(username);
            var receiver = await _userRepo.GetUserByUsernameAsync(dto.ReceiverUsername);

            if (receiver == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUsername = sender.UserName,
                ReceiverUsername = receiver.UserName,
                Content = dto.Content
            };

            _msgRepo.AddMessage(message);
            if (!await _msgRepo.SaveAllAsync()) return BadRequest("Failed to send message");

            return Ok(_mapper.Map<MessageDTO>(message));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams param)
        {
            param.Username = User.FindFirst(ClaimTypes.Name)?.Value;
            var msg = await _msgRepo.GetMessagesForUser(param);

            Response.AddPaginationHeader(new PaginationHeader(msg.CurrentPage,
                                                              msg.PageSize,
                                                              msg.TotalCount,
                                                              msg.TotalPages));
            return Ok(msg);
        }
    }
}
