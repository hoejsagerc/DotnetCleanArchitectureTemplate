using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Common.Interfaces.Authentication;
using Pokemon.Domain.AuthenticationAggregate;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly ILogger<LoginQueryHandler> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;


    public LoginQueryHandler(
        ILogger<LoginQueryHandler> logger,
        IJwtTokenGenerator jwtTokenGenerator,
        IHttpContextAccessor httpContextAccessor,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var loginResult = await _signInManager.PasswordSignInAsync(query.Email, query.Password, false, false);

        if(!loginResult.Succeeded)
        {
            _logger.LogError(
                "Login Failed - Invalid Credentials, {@UserEmail}, {@sourceIpAddress}, {@IsLockedOut}, {@IsNotAllowed}, {@RequiresTwoFactor}, {@DateTimUtc}",
                query.Email,
                sourceIpAddress,
                loginResult.IsLockedOut,
                loginResult.IsNotAllowed,
                loginResult.RequiresTwoFactor,
                DateTime.UtcNow);
            return Errors.User.InvalidCredentials;
        }

        var user = await _userManager.FindByEmailAsync(query.Email);

        var jwtToken = _jwtTokenGenerator.GenerateToken(user);

        _logger.LogInformation(
            "Login Successful - User successfully logged in, {@UserEmail}, {@sourceIpAddress}, {@DateTimeUtc}",
            query.Email,
            sourceIpAddress,
            DateTime.UtcNow);

        return new AuthenticationResult(
            user,
            jwtToken);
    }
}
