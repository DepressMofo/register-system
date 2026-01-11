using MediatR;
using Microsoft.AspNetCore.Mvc;
using RegisterSystem.Application.Common.DTOs.User;
using RegisterSystem.Application.Features.Users.Commands.LoginUser;
using RegisterSystem.Application.Features.Users.Commands.RegisterUser;

namespace RegisterSystem.API.Controllers
{
  public class UsersController(IMediator mediator) : ApiControllerBase(mediator)
  {
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
    {
      var command = new RegisterUserCommand(dto.FirstName, dto.LastName, dto.Email, dto.Password);
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
    {
      var command = new LoginUserCommand(dto.Email, dto.Password);
      var result = await _mediator.Send(command);
      return Ok(result);
    }
  }
}