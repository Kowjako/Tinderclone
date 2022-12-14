using DatingAppAPI.Application.Interfaces.Services;
using DatingAppAPI.Application.ServiceImpl;
using DatingAppAPI.Application.SignalR;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            // MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
