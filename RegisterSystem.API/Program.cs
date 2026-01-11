using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegisterSystem.API.Middleware;
using RegisterSystem.Application.Common.Interfaces;
using RegisterSystem.Application.Features.Users.Commands.LoginUser;
using RegisterSystem.Application.Features.Users.Commands.RegisterUser;
using RegisterSystem.Application.Features.Users.Queries.GetUserProfile;
using RegisterSystem.Domain.Entities;
using RegisterSystem.Infrastructure.Authentication;
using RegisterSystem.Infrastructure.Data;
using RegisterSystem.Infrastructure.Services;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// --- API Documentation & UI Context ---
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor(); // Required to access the current User inside non-controller classes

// --- Database Configuration (MariaDB/MySQL) ---
builder.Services.AddDbContext<ApplicationDbContext>((options) =>
{
  var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
  var serverVersion = ServerVersion.AutoDetect(connectionString);
  options.UseMySql(connectionString, serverVersion);
});

// --- Identity Core Configuration ---
// Defines password rules and links Identity to our DbContext and Roles
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
  options.Password.RequireDigit = false;
  options.Password.RequiredLength = 6;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// --- Authentication & JWT Bearer Setup ---
// Configures how the server validates the incoming JWT tokens
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
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"]!))
  };
});

builder.Services.AddAuthorization();

// --- MediatR Setup ---
// Registers all Handlers located in the Application assembly
builder.Services.AddMediatR((cfg) =>
{
  cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
  cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly);
  cfg.RegisterServicesFromAssembly(typeof(GetUserProfileQuery).Assembly);
});

// --- Custom Services & Dependency Injection ---
builder.Services.AddControllers();
builder.Services.AddScoped<IJwtProvider, JwtProvider>(); // Token generation logic
builder.Services.AddScoped<IUserContext, UserContext>(); // Bridge to get current User info

// --- Exception Handling & RFC 7807 ---
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Provides standard format for error responses

var app = builder.Build();

// --- Middleware Pipeline ---

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

// Error handling should be the first middleware to catch errors from the rest of the pipe
app.UseExceptionHandler();

app.UseAuthentication(); // Determines WHO the user is based on the JWT
app.UseAuthorization();  // Determines WHAT the user can do

app.MapControllers();

app.Run();