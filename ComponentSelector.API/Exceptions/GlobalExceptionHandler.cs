using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace ComponentSelector.API.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger, IHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");
        var errorResponse = new ErrorResponse
        {
            Message = exception.Message,
            Title = exception.GetType().Name,
        };
        if (env.IsDevelopment())
        {
            errorResponse.Details = exception.StackTrace;
        }
        switch (exception)
        {
            case BadHttpRequestException:
            case ConflictException:
            case IdentityException:
            case ValidationException:
                errorResponse.Status = (int)HttpStatusCode.BadRequest;
                break;
            case InvalidCredentialException:
                errorResponse.Status = (int)HttpStatusCode.Unauthorized;
                break;
            case InvalidConfigurationException:
            case InvalidOperationException:
            case ArgumentNullException:
                errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                break;
            default:
                errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                errorResponse.Title = "Internal Server Error";
                break;
        }
        httpContext.Response.StatusCode = errorResponse.Status;

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}
