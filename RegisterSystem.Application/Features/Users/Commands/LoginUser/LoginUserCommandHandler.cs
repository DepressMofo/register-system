using MediatR;
using Microsoft.AspNetCore.Identity;
using RegisterSystem.Application.Common.DTOs.User;
using RegisterSystem.Application.Common.Exceptions;
using RegisterSystem.Application.Common.Interfaces;
using RegisterSystem.Domain.Entities;

namespace RegisterSystem.Application.Features.Users.Commands.LoginUser
{
  public class LoginUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider
  ) : IRequestHandler<LoginUserCommand, ResponseUserDTO>
  {
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<ResponseUserDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
      var user = await _userManager.FindByEmailAsync(request.Email);

      if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
      {
        throw new ValidationException("Invalid email or password");
      }

      var token = _jwtProvider.GenerateToken(user, []);

      return new ResponseUserDTO(
        user.Email!,
        user.FullName,
        token
      );
    }
  }
}