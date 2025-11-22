using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BaskIt.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = GetTitle(statusCode),
            Detail = message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static string GetTitle(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.Unauthorized => "Unauthorized",
        HttpStatusCode.BadRequest => "Bad Request",
        HttpStatusCode.NotFound => "Not Found",
        _ => "Internal Server Error"
    };
}
