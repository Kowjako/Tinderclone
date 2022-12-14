using DatingAppAPI.Application.CQRS.Admin.Requests.Command;
using DatingAppAPI.Application.CQRS.Admin.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IMediator _mediatR;

        public AdminController(IMediator mediatr)
        {
            _mediatR = mediatr;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<List<UserWithRoleDTO>>> GetUsersWithRoles()
        {
            var result = await _mediatR.Send(new GetUsersWithRolesRequest());
            return Ok(result);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("edit-roles/{username}")]
        public async Task<ActionResult<List<string>>> EditRoles([FromRoute] string username, [FromQuery] string roles)
        {
            var result = await _mediatR.Send(new EditRolesRequest()
            {
                Username = username,
                Roles = roles
            });

            return Ok(result);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-approval")]
        public async Task<ActionResult<IEnumerable<PhotoForApprovalDTO>>> GetPhotosForApproval()
        {
            var result = await _mediatR.Send(new GetPhotosForApprovalRequest());
            return Ok(result);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("approve-photo/{photoId}")]
        public async Task<ActionResult<PhotoDTO>> ApprovePhoto([FromRoute] int photoId)
        {
            var result = await _mediatR.Send(new ApprovePhotoRequest() { PhotoId = photoId });
            return Ok(result);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto([FromRoute] int photoId)
        {
            await _mediatR.Send(new RejectPhotoRequest() { PhotoId = photoId });
            return NoContent();
        }
    }
}
