using DatingAppAPI.API.Extensions;
using DatingAppAPI.Application.CQRS.Message.Requests.Command;
using DatingAppAPI.Application.CQRS.Message.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMediator _mediatR;

        public MessagesController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage([FromBody] CreateMessageDTO dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _mediatR.Send(new CreateMessageRequest()
            {
                CreateMessageDTO = dto,
                Username = username
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDTO>>> GetMessagesForUser([FromQuery] MessageParams param)
        {
            param.Username = User.FindFirst(ClaimTypes.Name)?.Value;

            var msg = await _mediatR.Send(new GetMessagesForUserRequest()
            {
                Params = param
            });

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

            await _mediatR.Send(new DeleteMessageRequest()
            {
                Username = currentUserName,
                MessageId = msgId
            });

            return NoContent();
        }
    }
}
