using System.ComponentModel.DataAnnotations;

namespace RegisterSystem.Application.Common.DTOs.User
{
  public class LoginUserDTO
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
  }
}