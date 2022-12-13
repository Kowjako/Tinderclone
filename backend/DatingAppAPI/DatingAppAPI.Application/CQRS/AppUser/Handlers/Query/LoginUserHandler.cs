using DatingAppAPI.Application.CQRS.AppUser.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppU = DatingAppAPI.Domain.Entities.AppUser;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Query
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, UserDTO>
    {
        private readonly UserManager<AppU> _userMngr;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(UserManager<AppU> userMngr, ITokenService tokenService)
        {
            _userMngr = userMngr;
            _tokenService = tokenService;
        }

        public async Task<UserDTO> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userMngr.Users.Include(p => p.Photos)
                                            .SingleOrDefaultAsync(x => x.UserName.Equals(request.LoginDTO.Username));

            if (user == null) throw new HttpException(401, "Invalid username");

            var result = await _userMngr.CheckPasswordAsync(user, request.LoginDTO.Password);
            if (!result) throw new HttpException(401, "Invalid password");

            return new UserDTO()
            {
                Username = request.LoginDTO.Username,
                JwtToken = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
    }
}
