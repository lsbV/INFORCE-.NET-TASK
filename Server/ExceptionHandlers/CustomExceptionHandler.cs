using Microsoft.AspNetCore.Diagnostics;
using Server.Exceptions;
using ShortenerComponent.Exceptions;

namespace Server.ExceptionHandlers;

public class CustomExceptionHandler() : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case AccessToResourceIsForbidden:
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return ValueTask.FromResult(true);
            case UrlNotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return ValueTask.FromResult(true);
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return ValueTask.FromResult(true);
        }
    }
}