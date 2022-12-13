using AutoMapper;
using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppU = DatingAppAPI.Domain.Entities.AppUser;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Command
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, UserDTO>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppU> _userMngr;
        private readonly ITokenService _tokenService;

        public RegisterUserHandler(IMapper mapper, UserManager<AppU> userMngr, ITokenService tokenService)
        {
            _mapper = mapper;
            _userMngr = userMngr;
            _tokenService = tokenService;
        }

        public async Task<UserDTO> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            if (await UserExists(request.RegisterDTO.Username)) 
                throw new HttpException(400, "Username is taken");

            var user = _mapper.Map<AppU>(request.RegisterDTO);
            user.UserName = request.RegisterDTO.Username.ToLower();

            var saveUserResult = await _userMngr.CreateAsync(user, request.RegisterDTO.Password);
            if (!saveUserResult.Succeeded) throw new HttpException(400, "Cant save user");

            var roleResult = await _userMngr.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) throw new HttpException(400, "Cant add user to role");

            return new UserDTO()
            {
                Username = user.UserName,
                JwtToken = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userMngr.Users.AnyAsync(x => x.UserName.Equals(userName.ToLower()));
        }
    }
}
