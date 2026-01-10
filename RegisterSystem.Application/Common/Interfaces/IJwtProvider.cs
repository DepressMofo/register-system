using RegisterSystem.Domain.Entities;

namespace RegisterSystem.Application.Common.Interfaces
{
  public interface IJwtProvider
  {
    string GenerateToken(ApplicationUser user, IList<string> roles);
  }
}