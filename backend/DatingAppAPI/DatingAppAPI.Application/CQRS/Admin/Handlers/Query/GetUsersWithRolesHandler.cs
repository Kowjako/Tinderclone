using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppAPI.Application.CQRS.Admin.Requests.Query;
using DatingAppAPI.Application.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppU = DatingAppAPI.Domain.Entities.AppUser;

namespace DatingAppAPI.Application.CQRS.Admin.Handlers.Query
{
    internal class GetUsersWithRolesHandler : IRequestHandler<GetUsersWithRolesRequest, List<UserWithRoleDTO>>
    {
        private readonly UserManager<AppU> _userMngr;
        private readonly IMapper _mapper;

        public GetUsersWithRolesHandler(UserManager<AppU> userMngr, IMapper mapper)
        {
            _userMngr = userMngr;
            _mapper = mapper;
        }

        public async Task<List<UserWithRoleDTO>> Handle(GetUsersWithRolesRequest request, CancellationToken cancellationToken)
        {
            var users = await _userMngr.Users.OrderBy(u => u.UserName)
                                             .ProjectTo<UserWithRoleDTO>(_mapper.ConfigurationProvider)
                                             .ToListAsync();
            return users;
        }
    }
}
