using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RegisterSystem.API.Controllers
{
  [ApiController]
  [Route("/api/v1/[controller]")]
  public class ApiControllerBase(IMediator mediator) : ControllerBase
  {
    protected readonly IMediator _mediator = mediator;
  }
}