using DatingAppAPI.API.Extensions;
using DatingAppAPI.Application.CQRS.UserLike.Requests.Command;
using DatingAppAPI.Application.CQRS.UserLike.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Pagination;
using DatingAppAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IMediator _mediatR;

        public LikesController(IMediator mediatr)
        {
            _mediatR = mediatr;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _mediatR.Send(new AddLikeRequest()
            {
                Username = username,
                SourceUserId = int.Parse(sourceUserId)
            });
            
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikeParams likesParams)
        {
            var sourceUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var likes = await _mediatR.Send(new GetUserLikesRequest()
            {
                Params = likesParams,
                UserId = sourceUserId
            });

            Response.AddPaginationHeader(new PaginationHeader(likes.CurrentPage,
                                                              likes.PageSize,
                                                              likes.TotalCount,
                                                              likes.TotalPages));
            return Ok(likes);
        }
    }
}
