using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Commands.UpdatePassword;

public class UpdateUserPasswordCommandHandler
    : IRequestHandler<UpdateUserPasswordCommand, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

    public UpdateUserPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UpdateUserPasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        UpdateUserPasswordCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?
            .Connection.RemoteIpAddress?.ToString();

        var user = await _userManager.FindByIdAsync(command.Id);

        if (user is null)
        {
            _logger.LogError(
                "User Update failed - User not found, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.Id,
                sourceIpAddress,
                DateTime.UtcNow);
            return Errors.User.NotFound;
        }

        var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, command.Password);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "User Update Failed - Failed updating user in database, {@UserId}, {@UpdatedProperties}, {@Error}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.Id,
                "PasswordReset",
                result.Errors,
                sourceIpAddress,
                DateTime.UtcNow);

            return Errors.User.FailedUpdating;
        }

        _logger.LogInformation(
            "User Update Successful - User updated successfully, {@UserId}, {@UpdatedProperties}, {@SourceIpAddress}, {@DateTimeUtc}",
            command.Id,
            "PasswordReset",
            sourceIpAddress,
            DateTime.UtcNow);

        return new AuthenticationResult(user, "");
    }
}
