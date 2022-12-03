﻿using DatingAppAPI.Application.Interfaces.Services;
using DatingAppAPI.Application.ServiceImpl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DatingAppAPI.Application
{
    public static class DIConfigurator
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            return services;
        }
    }
}
