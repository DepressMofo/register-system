using MediatR;
using RegisterSystem.Application.Common.DTOs.User;

namespace RegisterSystem.Application.Features.Users.Commands.RegisterUser
{
  public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
  ) : IRequest<ResponseUserDTO>;
}