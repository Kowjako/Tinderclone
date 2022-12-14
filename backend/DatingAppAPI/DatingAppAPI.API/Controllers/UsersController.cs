using DatingAppAPI.API.Extensions;
using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.CQRS.AppUser.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediatR;

        public UsersController(IMediator mediatr)
        {
            _mediatR = mediatr;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUsers([FromQuery] UserParams param)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var users = await _mediatR.Send(new GetUsersRequest()
            {
                Params = param,
                Username = username
            });

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
                                                              users.TotalCount, users.TotalPages));
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUserById([FromRoute] string username)
        {
            var selfRequest = User.FindFirst(ClaimTypes.Name).Value.Equals(username);

            return Ok(await _mediatR.Send(new GetUserByIdRequest()
            {
                Username = username,
                SelfRequest = selfRequest
            }));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpd)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            await _mediatR.Send(new UpdateUserRequest()
            {
                Username = username,
                MemberUpdateDTO = memberUpd
            });
            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _mediatR.Send(new AddPhotoRequest()
            {
                Username = username,
                File = file
            });

            if (result != null)
            {
                return CreatedAtAction(nameof(GetUserById),
                                       new { username = username },
                                       result);
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _mediatR.Send(new SetMainPhotoRequest()
            {
                Username = username,
                PhotoId = photoId
            });

            if(result)
            {
                return NoContent();
            }

            return BadRequest("Problem setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            await _mediatR.Send(new DeletePhotoRequest()
            {
                Username = username,
                PhotoId = photoId
            });

            return NoContent();
        }
    }
}
