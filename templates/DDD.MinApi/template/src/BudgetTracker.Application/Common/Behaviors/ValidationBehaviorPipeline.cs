using BudgetTracker.Application.Common.Services;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BudgetTracker.Application.Common.Behaviors;

public class ValidationBehaviorPipeline<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> 
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILoggerAdapter<ValidationBehaviorPipeline<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ValidationBehaviorPipeline(
        ILoggerAdapter<ValidationBehaviorPipeline<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor, 
        IDateTimeProvider dateTimeProvider,IValidator<TRequest>? validator = null)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid)
        {
            return await next();
        }

        foreach (var error in validationResult.Errors)
        {
            _logger.Error("Validation Error {@Error}, {@SourceIpAddress}, {@DateTimeUtc}",
                error, sourceIpAddress ?? "n/a", _dateTimeProvider.UtcNow);
        }

        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage));

        return (dynamic)errors;
    }
}