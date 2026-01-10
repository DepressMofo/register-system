using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using RegisterSystem.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>((options) =>
{
  var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
  var serverVersion = ServerVersion.AutoDetect(connectionString);
  options.UseMySql(connectionString, serverVersion);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();