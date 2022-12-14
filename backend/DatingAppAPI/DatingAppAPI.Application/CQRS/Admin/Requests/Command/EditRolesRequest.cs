using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Requests.Command
{
    public class EditRolesRequest : IRequest<IEnumerable<string>>
    {
        public string Username { get; set; }
        public string Roles { get; set; }
    }
}
