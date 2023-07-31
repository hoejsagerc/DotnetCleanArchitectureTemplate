using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Common.Interfaces.Authentication;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace Pokemon.Application.Authentication.v1.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<LoginQueryHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var user = await _userRepository.GetUserByEmailAsync(query.Email);

        if (user is null)
        {
            _logger.LogWarning(
                "Login failed - User does not exist, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}",
                query.Email,
                sourceIpAddress,
                DateTime.UtcNow);
            return Errors.Authentication.InvalidCredentials;
        }


        if (!PasswordHash.Verify(password: query.Password, hashedPassword: user.HashedPassword))
        {
            _logger.LogWarning(
                "Login failed - Incorrect password provided, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}",
                query.Email,
                sourceIpAddress,
                DateTime.UtcNow);

            await HandleUserFailedLogins(user, sourceIpAddress, query);

            return new[] { Errors.Authentication.InvalidCredentials };
        }

        if (user.UserLockoutEnabled && user.LockedOut && user.LockoutEnd > DateTime.UtcNow)
        {
            _logger.LogWarning(
                "Login Failed - User is locked, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}, {@LockOutAttempts}, {@LockoutEnd}",
                query.Email,
                sourceIpAddress,
                DateTime.UtcNow,
                user.FailedLogins,
                user.LockoutEnd);

            return new[] { Errors.Authentication.InvalidCredentials };
        }
        else if (user.UserLockoutEnabled && user.LockedOut && user.LockoutEnd < DateTime.UtcNow)
        {
            user.UnlockUser();
            await _userRepository.UpdateAsync(user);
        }

        (string token, RefreshToken refreshToken) = await GenerateTokens(user, query, sourceIpAddress);

        return new AuthenticationResult(
            user,
            token,
            refreshToken.Token);
    }


    private async Task<(string, RefreshToken)> GenerateTokens(
        User user,
        LoginQuery query,
        string? sourceIpAddress)
    {
        var token = _jwtTokenGenerator.GenerateToken(user);

        var refreshToken = RefreshToken.Create(
            DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("RefreshTokenSettings:ExpiryDays")),
            UserId.Create(Guid.Parse(user.Id.ToString()!)));
        await _refreshTokenRepository.AddAsync(refreshToken);

        _logger.LogInformation(
            "Login successful, {@UserEmail}, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
            query.Email,
            user.Id,
            sourceIpAddress,
            DateTime.UtcNow);

        return (token, refreshToken);
    }


    private async Task HandleUserFailedLogins(
        User user, string? sourceIpAddress, LoginQuery query)
    {
        if (user.UserLockoutEnabled)
        {
            var oldUserLockoutState = user.LockedOut;

            // Check the user login attempts and maybe lockout the user
            user.CheckUserLockoutState();

            // Check if the user was locked out and if so the log it
            var newUserLockoutState = user.LockedOut;
            if (newUserLockoutState && oldUserLockoutState != newUserLockoutState)
            {
                _logger.LogWarning(
                    "User Locked - To many failed login attempts, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}, {@LockOutAttempts}, {@LockoutEnd}",
                    query.Email,
                    sourceIpAddress,
                    DateTime.UtcNow,
                    user.FailedLogins,
                    user.LockoutEnd);
            }

            // check if the user was already locked out and if so the log it
            if (newUserLockoutState && oldUserLockoutState == newUserLockoutState)
            {
                _logger.LogWarning(
                    "Login Failed - User is locked, {@UserEmail}, {@SourceIpAddress}, {@DateTimeUtc}, {@LockOutAttempts}, {@LockoutEnd}",
                    query.Email,
                    sourceIpAddress,
                    DateTime.UtcNow,
                    user.FailedLogins,
                    user.LockoutEnd);
            }

            // update the user in the repository
            await _userRepository.UpdateAsync(user);
        }
    }

}
