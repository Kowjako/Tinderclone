using DatingAppAPI.DTO;
using DatingAppAPI.Persistance.Data;
using DatingAppAPI.Persistance.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dbContext;

        public AccountController(DataContext context)
        {
            _dbContext = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register([FromBody]RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login([FromBody]LoginDTO loginDTO)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName.Equals(loginDTO.Username));

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid password");

            return Ok(user);
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName.Equals(userName.ToLower()));
        }
    }
}
