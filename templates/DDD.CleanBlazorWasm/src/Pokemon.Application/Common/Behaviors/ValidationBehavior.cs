using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Pokemon.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public ValidationBehavior(
        ILogger<ValidationBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor,
        IValidator<TRequest>? validator = null)
    {
        _validator = validator;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
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
        else {
            foreach (var error in validationResult.Errors)
            {
                _logger.LogError("Validation error {@RequestName}, {@Error}, {@SourceIpAddress}, {@DateTimeUtc}",
                    typeof(TRequest).Name, sourceIpAddress, error, DateTime.UtcNow);
            }
        }

        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage));

        return (dynamic)errors;
    }
}