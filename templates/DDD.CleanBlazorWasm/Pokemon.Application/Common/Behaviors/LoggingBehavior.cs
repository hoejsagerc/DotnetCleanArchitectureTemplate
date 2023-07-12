using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Pokemon.Application.Common.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{

    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingPipelineBehavior(
        ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        _logger.LogInformation("Starting request {@RequestName}, {@SourceIpAddress}, {@DateTimeUtc}",
            typeof(TRequest).Name, sourceIpAddress, DateTime.UtcNow);

        var result = await next();

        if (result.IsError)
        {
            foreach (var error in result.Errors!)
            {
                _logger.LogError("Request failure {@RequestName},{@SourceIpAddress}, {@Error}, {@DateTimeUtc}",
                    typeof(TRequest).Name, sourceIpAddress, error, DateTime.UtcNow);
            }
        }

        _logger.LogInformation("Completed request {@RequestName}, {@SourceIpAddress}, {@DateTimeUtc}",
            typeof(TRequest).Name, sourceIpAddress, DateTime.UtcNow);

        return result;
    }
}