using MediatR;
using Microsoft.AspNetCore.Identity;
using RegisterSystem.Application.Common.DTOs.User;
using RegisterSystem.Application.Common.Exceptions;
using RegisterSystem.Application.Common.Interfaces;
using RegisterSystem.Domain.Entities;

namespace RegisterSystem.Application.Features.Users.Commands.RegisterUser
{
  public class RegisterUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider
  ) : IRequestHandler<RegisterUserCommand, ResponseUserDTO>
  {
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<ResponseUserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

      var existingUser = await _userManager.FindByEmailAsync(request.Email);

      if (existingUser is not null) throw new ValidationException("email already registered");

      var user = new ApplicationUser(request.FirstName, request.LastName)
      {
        Email = request.Email,
        UserName = request.Email
      };

      var result = await _userManager.CreateAsync(user, request.Password);

      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select((e) => e.Description));
        throw new ValidationException($"Registration failed: {errors}");
      }

      var token = _jwtProvider.GenerateToken(user, []);
      return new ResponseUserDTO(
        user.Email,
        user.FullName,
        token
      );
    }
  }
}