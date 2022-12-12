using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingAppAPI.API.Filter
{
    public class LogUserActivityFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await uow.UserRepository.GetUserByIdAsync(int.Parse(userId));
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}
