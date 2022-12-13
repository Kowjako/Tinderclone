using AutoMapper;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Services;
using DatingAppAPI.Controllers;
using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userMngr;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPhotoService _phService;

        public AdminController(IUnitOfWork uow, UserManager<AppUser> userMngr, IMapper mapper, IPhotoService phService)
        {
            _userMngr = userMngr;
            _uow = uow;
            _mapper = mapper;
            _phService = phService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userMngr.Users.OrderBy(u => u.UserName)
                                             .Select(p => new
                                             {
                                                 p.Id,
                                                 Username = p.UserName,
                                                 Roles = p.UserRoles.Select(p => p.Role.Name).ToList()
                                             })
                                             .ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("edit-roles/{username}")]
        public async Task<ActionResult<List<string>>> EditRoles([FromRoute] string username, [FromQuery] string roles)
        {
            if (string.IsNullOrWhiteSpace(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",");
            var user = await _userMngr.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await _userMngr.GetRolesAsync(user);
            var result = await _userMngr.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            result = await _userMngr.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");
            return Ok(await _userMngr.GetRolesAsync(user));
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-approval")]
        public async Task<ActionResult<IEnumerable<PhotoForApprovalDTO>>> GetPhotosForApproval()
        {
            return Ok(await _uow.PhotoRepository.GetUnapprovedPhotos());
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("approve-photo/{photoId}")]
        public async Task<ActionResult<PhotoDTO>> ApprovePhoto([FromRoute] int photoId)
        {
            var photo = await _uow.PhotoRepository.GetPhotoById(photoId);

            if (photo == null) return NotFound("Could not find photo");

            photo.IsApproved = true;

            var user = await _uow.UserRepository.GetUserByPhotoId(photoId);
            if (!user.Photos.Any(p => p.IsMain)) photo.IsMain = true;

            if (!await _uow.Complete()) return BadRequest("Fail to change photo status");
            return _mapper.Map<PhotoDTO>(photo);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("reject-photo/{photoId}")]
        public async Task<ActionResult<PhotoDTO>> RejectPhoto([FromRoute] int photoId)
        {
            var photo = await _uow.PhotoRepository.GetPhotoById(photoId);

            if(photo.PublicId != null)
            {
                /* Remove from Cloudinary */
                var result = await _phService.DeletePhotoAsync(photo.PublicId);
                if(result.Result.Equals("ok"))
                {
                    _uow.PhotoRepository.DeletePhoto(photo);
                }
            }
            else
            {
                _uow.PhotoRepository.DeletePhoto(photo);
            }

            if (!await _uow.Complete()) return BadRequest("Fail to change photo status");
            return NoContent();
        }
    }
}
