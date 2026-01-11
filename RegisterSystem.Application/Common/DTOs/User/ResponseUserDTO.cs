namespace RegisterSystem.Application.Common.DTOs.User
{
  public record ResponseUserDTO(
    string Email,
    string FullName,
    string Token
  );
}