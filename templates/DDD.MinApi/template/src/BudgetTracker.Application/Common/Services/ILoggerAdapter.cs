namespace BudgetTracker.Application.Common.Services;

public interface ILoggerAdapter<T>
{
    public void Debug(string? message, params object?[] args);
    public void Information(string? message, params object?[] args);
    public void Error(string? message, params object?[] args);
    public void Warning(string? message, params object?[] args);
    public void Fatal(string? message, params object?[] args);
    public void Verbose(string? message, params object?[] args);
}