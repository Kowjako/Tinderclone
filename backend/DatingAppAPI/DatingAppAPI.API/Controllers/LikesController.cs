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
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;

        public LikesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var likedUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _uow.LikesRepository.GetUserWithLikes(int.Parse(sourceUserId));

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName.Equals(username)) return BadRequest("You cannot like yourself");

            var userLike = await _uow.LikesRepository.GetUserLike(int.Parse(sourceUserId), likedUser.Id);
            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike()
            {
                SourceUserId = int.Parse(sourceUserId),
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            if (await _uow.Complete()) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikeParams likesParams)
        {
            var sourceUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            likesParams.UserId = int.Parse(sourceUserId);

            var likes = await _uow.LikesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(likes.CurrentPage,
                                                              likes.PageSize,
                                                              likes.TotalCount,
                                                              likes.TotalPages));

            return Ok(likes);
        }
    }
}
