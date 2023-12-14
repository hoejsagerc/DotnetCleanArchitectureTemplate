using BudgetTracker.Application.Common.Services;
using Serilog;

namespace BudgetTracker.Infrastructure.Services;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
    private readonly Serilog.ILogger _logger;

    public LoggerAdapter(ILogger logger)
    {
        _logger = logger;
    }


    public void Debug(string? message, params object?[] args)
    {
        _logger.Debug(message?? string.Empty, args);
    }

    public void Information(string? message, params object?[] args)
    {
        _logger.Information(message?? string.Empty, args);
    }

    public void Error(string? message, params object?[] args)
    {
        _logger.Error(message ?? string.Empty, args);
    }

    public void Warning(string? message, params object?[] args)
    {
        _logger.Warning(message ?? string.Empty, args);
    }

    public void Fatal(string? message, params object?[] args)
    {
        _logger.Fatal(message ?? string.Empty, args);
    }

    public void Verbose(string? message, params object?[] args)
    {
        _logger.Verbose(message ?? string.Empty, args);
    }
}