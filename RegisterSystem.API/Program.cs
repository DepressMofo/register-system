using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddAuthentication((options) =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer((options) =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT_ISSUER"],
    ValidAudience = builder.Configuration["JWT_AUDIENCE"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_KEY"]!))
  };
});
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();