using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Domain.AuthenticationAggregate;

namespace Pokemon.Application.Authentication.v1.Commands.Register;

public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<RegisterCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?
            .Connection.RemoteIpAddress?.ToString();

        var existingUser = await _userManager.FindByEmailAsync(command.Email);

        if(existingUser is not null)
        {
            _logger.LogWarning("User registration failed - Email address already exists, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.Email, sourceIpAddress, DateTime.UtcNow);
            return Errors.User.DuplicateEmail;
        }

        var newUser = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email
        };

        var result = await _userManager.CreateAsync(newUser, command.Password);

        if(!result.Succeeded)
        {
            _logger.LogError("User registration failed - Failed adding new user to database, {@UserEmail}, {@SourceIpAddress}, {@Error}, {@DateTimeUtc]}",
                command.Email, sourceIpAddress, result.Errors, DateTime.UtcNow);

            if (result.Errors.Any(e => e.Code == "PasswordRequiresNonAlphanumeric"))
                return Errors.User.PasswordValidationFailed;

            return Errors.User.FailedCreating;
        }

        _logger.LogInformation("User registration successful - User successfully registerd, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}",
            command.Email, sourceIpAddress, DateTime.UtcNow);

        return new AuthenticationResult(newUser, "");
    }
}