using BudgetTracker.Application.Common.Services;
using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace BudgetTracker.Application.Common.Behaviors;

public class LoggingBehaviorPipeline<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> 
    where TResponse : IErrorOr
{
    private readonly ILoggerAdapter<LoggingBehaviorPipeline<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoggingBehaviorPipeline(ILoggerAdapter<LoggingBehaviorPipeline<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        
        _logger.Information("Starting request {@RequestName}, {@SourceIpAddress}, {@DateTimeUtc}",
            typeof(TRequest).Name, sourceIpAddress ?? "n/a", _dateTimeProvider.UtcNow);

        var result = await next();

        if (result.IsError)
        {
            foreach (var error in result.Errors!)
            {
                _logger.Error("Request error: {@RequestName}, {@SourceIpAddress}, {@DateTimeUtc}, {@Error}",
                    typeof(TRequest).Name, sourceIpAddress ?? "n/a", _dateTimeProvider.UtcNow, error);
            }
            
            _logger.Information("Completed request with errors {@RequestName}, {@SourceIpAddress}, {@DateTimeUtc}",
                typeof(TRequest).Name, sourceIpAddress ?? "n/a", _dateTimeProvider.UtcNow);
        }
        else
        {
            _logger.Information("Completed request {@RequestName}, {@SourceIpAddress}, {@DateTimeUtc}",
                typeof(TRequest).Name, sourceIpAddress ?? "n/a", _dateTimeProvider.UtcNow);
        }

        return result;
    }
}