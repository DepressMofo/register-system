using MediatR;
using RegisterSystem.Application.Common.DTOs.User;

namespace RegisterSystem.Application.Features.Users.Commands.LoginUser
{
  public record LoginUserCommand(string Email, string Password) : IRequest<ResponseUserDTO>;
}