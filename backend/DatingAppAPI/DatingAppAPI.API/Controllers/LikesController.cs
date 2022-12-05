using DatingAppAPI.API.Extensions;
using DatingAppAPI.Application.DTO;
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
        private readonly IUserRepository _userRepo;
        private ILikesRepository _likeRepo;

        public LikesController(IUserRepository userRepo, ILikesRepository likeRepo)
        {
            _userRepo = userRepo;
            _likeRepo = likeRepo;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var likedUser = await _userRepo.GetUserByUsernameAsync(username);
            var sourceUser = await _likeRepo.GetUserWithLikes(int.Parse(sourceUserId));

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName.Equals(username)) return BadRequest("You cannot like yourself");

            var userLike = await _likeRepo.GetUserLike(int.Parse(sourceUserId), likedUser.Id);
            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike()
            {
                SourceUserId = int.Parse(sourceUserId),
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            if (await _userRepo.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikeParams likesParams)
        {
            var sourceUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            likesParams.UserId = int.Parse(sourceUserId);

            var likes = await _likeRepo.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(likes.CurrentPage,
                                                              likes.PageSize,
                                                              likes.TotalCount,
                                                              likes.TotalPages));

            return Ok(likes);
        }
    }
}
