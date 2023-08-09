using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Commands.UpdateUser;

public class UpdateUserEmailCommandHandler
    : IRequestHandler<UpdateUserEmailCommand, ErrorOr<AuthenticationResult>>
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UpdateUserEmailCommandHandler> _logger;

    public UpdateUserEmailCommandHandler(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UpdateUserEmailCommandHandler> logger)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        UpdateUserEmailCommand command,
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

        // (ApplicationUser updatedUser, string updatedProperties) = UpdateUser(user, command);
        user.Email = command.Email;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "User Update Failed - Failed updating user in database, {@UserId}, {@UpdatedProperties}, {@Error}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.Id,
                "EmailUpdate",
                result.Errors,
                sourceIpAddress,
                DateTime.UtcNow);

            return Errors.User.FailedUpdating;
        }

        _logger.LogInformation(
            "User Update Successful - User updated successfully, {@UserId}, {@UpdatedProperties}, {@SourceIpAddress}, {@DateTimeUtc}",
            command.Id,
            "EmailUpdate",
            sourceIpAddress,
            DateTime.UtcNow);

        return new AuthenticationResult(user, "");
    }

    // private (ApplicationUser, string) UpdateUser(
    //     ApplicationUser user,
    //     UpdateUserEmailCommand command)
    // {
    //     List<string> updatedProperties = new();

    //     if (command.Email is not null)
    //     {
    //         user.Email = command.Email;
    //         user.UserName = command.Email;
    //         updatedProperties.Add("email");
    //     }

    //     if (command.GivenName is not null)
    //     {
    //         user.GivenName = command.GivenName;
    //         updatedProperties.Add("givenname");
    //     }

    //     if (command.Surname is not null)
    //     {
    //         user.Surname = command.Surname;
    //         updatedProperties.Add("surname");
    //     }

    //     string updatedPropertiesString = string.Join(",", updatedProperties);

    //     return (user, updatedPropertiesString);
    // }
}
