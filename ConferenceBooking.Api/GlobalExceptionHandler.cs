using ConferenceBooking.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api;

// One place to turn domain and application failures into status codes, so controllers
// stay free of try/catch and no internal detail leaks into a response.
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = Map(exception);

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception processing {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = statusCode;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                // Only expose the message for failures the caller can act on.
                Detail = statusCode == StatusCodes.Status500InternalServerError ? null : exception.Message,
            },
        });
    }

    private static (int StatusCode, string Title) Map(Exception exception) => exception switch
    {
        NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
        ConflictException => (StatusCodes.Status409Conflict, "Conflict"),

        // Domain guards throw these, so a bad value reaching the entity is still a 400.
        ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request"),

        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred"),
    };
}
