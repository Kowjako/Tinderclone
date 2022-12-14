using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.CQRS.AppUser.Requests.Query;
using DatingAppAPI.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DatingAppAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IMediator _mediatR;

        public AccountController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _mediatR.Send(new RegisterUserRequest() { RegisterDTO = registerDTO });
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _mediatR.Send(new LoginUserRequest() { LoginDTO = loginDTO });
            return Ok(result);
        }
    }
}
