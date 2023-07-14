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

namespace Pokemon.Application.Authentication.v1.Commands.Refresh;

public class RefreshCommandHandler :
    IRequestHandler<RefreshCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<RefreshCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshCommandHandler(
        IUserRepository userRepository,
        ILogger<RefreshCommandHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RefreshCommand command,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?
            .Connection.RemoteIpAddress?.ToString();

        var user = await _userRepository.GetUserByIdAsync(
            UserId.Create(Guid.Parse(command.UserId)));

        if (user is null)
        {
            _logger.LogError(
                "Failed refreshing token - user was not found, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                sourceIpAddress,
                DateTime.UtcNow);

            return Errors.User.NotFound;
        }

        var userRefreshToken = await _refreshTokenRepository.GetById(
            RefreshTokenId.Create(Guid.Parse(user.RefreshTokenId!.ToString()!)));

        if (userRefreshToken is null)
        {
            _logger.LogWarning(
                "Failed refreshing token - refresh token was no found, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                sourceIpAddress,
                DateTime.UtcNow);
            return Errors.RefreshToken.NotFound;
        }

        if (!CheckRefreshToken(userRefreshToken, command, sourceIpAddress))
        {
            return Errors.RefreshToken.InvalidRefreshToken;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        _logger.LogInformation(
            "Refreshed token successful, {@UserId}, {@RefreshTokenId}, {@SourceIpAddress}, {@DateTimeUtc}",
            command.UserId,
            userRefreshToken.Id,
            sourceIpAddress,
            DateTime.UtcNow);

        return new AuthenticationResult(
            user,
            token,
            command.RefreshToken);
    }


    private bool CheckRefreshToken(
        RefreshToken userRefreshToken,
        RefreshCommand command,
        string? sourceIpAddress)
    {

        if (userRefreshToken.Token != command.RefreshToken)
        {
            _logger.LogError(
                "Failed refreshing token - provided token was not equal to user assigned token}, {@UserId}, {@RefreshTokenId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                userRefreshToken.Id,
                sourceIpAddress,
                DateTime.UtcNow);
            return false;
        }

        if (userRefreshToken.Revoked)
        {
            _logger.LogError(
                "Failed refreshing token - refresh token has been revoked}, {@UserId}, {@RefreshTokenId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                userRefreshToken.Id,
                sourceIpAddress,
                DateTime.UtcNow);
            return false;
        }

        if (userRefreshToken.Expires < DateTime.UtcNow)
        {
            _logger.LogError(
                "Failed refreshing token - refresh token is expired, {@UserId}, {@RefreshTokenId}, {@SourceIpAddress}, {@DateTimeUtc}",
                command.UserId,
                userRefreshToken.Id,
                sourceIpAddress,
                DateTime.UtcNow);
            return false;
        }

        return true;
    }
}
