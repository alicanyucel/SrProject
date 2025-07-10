using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace TELERADIOLOGY.Application.Behaviors;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly string RequestName = typeof(TRequest).Name;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var userId = GetUserId();
        var userName = GetUserName();

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = Guid.NewGuid(),
            ["RequestName"] = RequestName,
            ["UserId"] = userId,
            ["UserName"] = userName,
            ["Timestamp"] = DateTime.UtcNow
        }))
        {
            _logger.LogInformation("Handling {RequestName} {@Request} by User {UserId}",
                RequestName, request, userId);

            try
            {
                var response = await next();

                stopwatch.Stop();
                _logger.LogInformation("Handled {RequestName} successfully in {ElapsedMs}ms by User {UserName}",
                    RequestName, stopwatch.ElapsedMilliseconds, userName);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error handling {RequestName} after {ElapsedMs}ms by User {UserId}",
                    RequestName, stopwatch.ElapsedMilliseconds, userId);
                throw;
            }
        }
    }

    private string GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   user.FindFirst("sub")?.Value ??
                   user.FindFirst("id")?.Value ??
                   "Anonymous";
        }
        return "Anonymous";
    }

    private string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value ??
                   user.FindFirst("name")?.Value ??
                   user.FindFirst("username")?.Value ??
                   "Unknown";
        }
        return "Anonymous";
    }
}