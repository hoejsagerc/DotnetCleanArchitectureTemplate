using System.Text;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ErrorOr<AuthenticationResult>>
{
    private readonly ILogger<VerifyEmailCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public VerifyEmailCommandHandler(
        ILogger<VerifyEmailCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        VerifyEmailCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user is null)
        {
            _logger.LogError(
                "Email Verification Failed - User not found, {@UserId}, {@sourceIpAddress}, {@DateTimUtc}",
                command.UserId,
                sourceIpAddress,
                DateTime.UtcNow);
            return Errors.User.NotFound;
        }

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            _logger.LogError(
                "Email Verification Failed - Unexpected error occured, {@UserId}, {@sourceIpAddress}, {@DateTimUtc}",
                command.UserId,
                sourceIpAddress,
                DateTime.UtcNow);
            return Errors.User.EmailVerificationFailed;
        }

        return new AuthenticationResult(user, "");
    }
}
