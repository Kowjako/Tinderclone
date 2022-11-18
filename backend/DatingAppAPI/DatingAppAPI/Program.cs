using DatingAppAPI.Extensions;
using DatingAppAPI.Interfaces;
using DatingAppAPI.Persistence;
using DatingAppAPI.ServiceImpl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/* DI - container */

builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.ConfigureIdentity(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var app = builder.Build();

/* Http Request Pipeline */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS for Angular App
app.UseCors(x => x.AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(ep => ep.MapControllers());
app.Run();
