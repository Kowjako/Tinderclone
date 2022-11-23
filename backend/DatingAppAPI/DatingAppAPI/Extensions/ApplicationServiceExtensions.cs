using DatingAppAPI.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAppAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection RegisterMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ExceptionMiddleware>();

            return services;
        }
    }
}
