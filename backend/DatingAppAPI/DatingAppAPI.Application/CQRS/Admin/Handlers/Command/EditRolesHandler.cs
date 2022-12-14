using DatingAppAPI.Application.CQRS.Admin.Requests.Command;
using DatingAppAPI.Application.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AppU = DatingAppAPI.Domain.Entities.AppUser;

namespace DatingAppAPI.Application.CQRS.Admin.Handlers.Command
{
    public class EditRolesHandler : IRequestHandler<EditRolesRequest, IEnumerable<string>>
    {
        private readonly UserManager<AppU> _userMngr;

        public EditRolesHandler(UserManager<AppU> userMngr)
        {
            _userMngr = userMngr;
        }

        public async Task<IEnumerable<string>> Handle(EditRolesRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Roles)) 
                throw new HttpException(400, "You must select at least one role");

            var selectedRoles = request.Roles.Split(",");
            var user = await _userMngr.FindByNameAsync(request.Username);

            if (user == null) throw new HttpException(404, string.Empty);

            var userRoles = await _userMngr.GetRolesAsync(user);
            var result = await _userMngr.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) throw new HttpException(400, "Failed to add to roles");
            result = await _userMngr.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) throw new HttpException(400, "Failed to remove from roles");

            return await _userMngr.GetRolesAsync(user);
        }
    }
}
