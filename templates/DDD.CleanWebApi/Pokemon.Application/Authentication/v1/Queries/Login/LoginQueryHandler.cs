using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Common.Interfaces.Authentication;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.DomainErrors;
using Pokemon.Domain.UserAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Application.Authentication.v1.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginQueryHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<LoginQueryHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenRepository = refreshTokenRepository;
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
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        // await RevokeOldRefreshToken(user, sourceIpAddress);

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
            DateTime.UtcNow.AddDays(7),
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


    // private async Task RevokeOldRefreshToken(User user, string? sourceIpAddress)
    // {
    //     var oldRefreshToken = await _refreshTokenRepository.GetByUserIdAsync(
    //         UserId.Create(Guid.Parse(user.Id.ToString()!)));

    //     if (oldRefreshToken is not null)
    //     {
    //         oldRefreshToken.Revoke();

    //         await _refreshTokenRepository.UpdateTokenAsync(oldRefreshToken);

    //         _logger.LogInformation(
    //             "User refresh token revoked successful, {@RefreshTokenId}, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
    //             oldRefreshToken.Id,
    //             user.Id,
    //             sourceIpAddress,
    //             DateTime.UtcNow);
    //     }
    // }
}
