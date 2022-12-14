using DatingAppAPI.Application.DTO;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Requests.Query
{
    public class GetUsersWithRolesRequest : IRequest<List<UserWithRoleDTO>>
    {
    }
}
