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

        public AdminController(UserManager<AppUser> userMngr)
        {
            _userMngr = userMngr;
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

        [Authorize(Policy="RequireAdminRole")]
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
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admin or moderators can see this");
        }
    }
}
