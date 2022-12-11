using DatingAppAPI.Application.Interfaces.Services;
using DatingAppAPI.Application.ServiceImpl;
using DatingAppAPI.Application.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingAppAPI.Application
{
    public static class DIConfigurator
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();

            // SignalR
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();

            return services;
        }
    }
}
