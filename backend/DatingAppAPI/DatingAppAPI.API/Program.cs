using DatingAppAPI.Application;
using DatingAppAPI.Extensions;
using DatingAppAPI.Middlewares;
using DatingAppAPI.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

/* DI - container */

builder.Services.ConfigurePersistence(builder.Configuration); // App.Persistence
builder.Services.ConfigureIdentity(builder.Configuration); // JWT Identity
builder.Services.RegisterMiddlewares(); // Middlewares
builder.Services.ConfigureApplication(builder.Configuration); // App.Application

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

/* Http Request Pipeline */

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.SeedDatabaseAsync(); // fill test db on dev workspace
}

app.UseMiddleware<ExceptionMiddleware>();

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
