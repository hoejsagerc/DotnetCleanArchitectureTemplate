using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LogoutCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutCommandHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<LogoutCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<string>> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        var user = await _userManager.FindByIdAsync(command.UserId);

        if(user is null)
        {
            _logger.LogError(
                "User Logout Failed - User not found {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                sourceIpAddress,
                DateTime.UtcNow);
            return Errors.User.NotFound;
        }


            _logger.LogInformation(
                "User Logout Successful - User logged out {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                sourceIpAddress,
                DateTime.UtcNow);

        return "";
    }
}
