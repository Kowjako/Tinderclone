using AutoMapper;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Services;
using DatingAppAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userMngr;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userMngr, ITokenService tokenService, IMapper mapper)
        {
            _userMngr = userMngr;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDTO);
            user.UserName = registerDTO.Username.ToLower();

            var saveUserResult = await _userMngr.CreateAsync(user, registerDTO.Password);
            if (!saveUserResult.Succeeded) return BadRequest(saveUserResult.Errors);

            var roleResult = await _userMngr.AddToRoleAsync(user, "Member");
            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok(new UserDTO()
            {
                Username = user.UserName,
                JwtToken = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody]LoginDTO loginDTO)
        {
            var user = await _userMngr.Users.Include(p => p.Photos)
                                            .SingleOrDefaultAsync(x => x.UserName.Equals(loginDTO.Username));

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userMngr.CheckPasswordAsync(user, loginDTO.Password);
            if (!result) return Unauthorized("Invalid password");

            return Ok(new UserDTO()
            {
                Username = loginDTO.Username,
                JwtToken = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            });
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userMngr.Users.AnyAsync(x => x.UserName.Equals(userName.ToLower()));
        }
    }
}
