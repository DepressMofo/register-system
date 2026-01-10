using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegisterSystem.Application.Common.Interfaces;
using RegisterSystem.Domain.Entities;

namespace RegisterSystem.Infrastructure.Authentication
{
  public class JwtProvider(IConfiguration configuration) : IJwtProvider
  {
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
      var claims = new List<Claim>
      {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Email, user.Email!),
        new("FirstName", user.FirstName),
        new("LastName", user.LastName)
      };

      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]!));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: _configuration["JWT_ISSUER"],
        audience: _configuration["JWT_AUDIENCE"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(10),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}