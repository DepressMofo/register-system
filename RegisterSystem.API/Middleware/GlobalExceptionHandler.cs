using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RegisterSystem.Application.Common.Exceptions;

namespace RegisterSystem.API.Middleware
{
  public class GlobalExceptionHandler : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken
      )
    {
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Server Error :(",
        Detail = "An unexpected error ocurred, please contact support.",
        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
      };

      if (exception is Application.Common.Exceptions.ValidationException)
      {
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "One or more validation errors occurred.";
        problemDetails.Detail = exception.Message;
      }

      if (exception is EmailNotFoundException)
      {
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "One or more validation errors occurred.";
        problemDetails.Detail = exception.Message;
      }

      httpContext.Response.StatusCode = problemDetails.Status.Value;
      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

      return true;
    }
  }
}