using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.Events;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Application.Authentication.v1.Events;

public class RefreshTokenCreatedEventHandler : INotificationHandler<RefreshTokenCreated>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RefreshTokenCreatedEventHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCreatedEventHandler(
        IUserRepository userRepository,
        ILogger<RefreshTokenCreatedEventHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task Handle(
        RefreshTokenCreated refreshTokenCreatedEvent,
        CancellationToken cancellationToken)
    {
        string? sourceIpAddress = _httpContextAccessor.HttpContext?
            .Connection.RemoteIpAddress?.ToString();

        var userId = UserId.Create(
            Guid.Parse(
                refreshTokenCreatedEvent.RefreshToken.UserId.ToString()!));

        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null)
        {
            _logger.LogWarning(
                "User refresh token update failed - user was not found, {@RefreshTokenId}, {@SourceIpAddress}, {@DateTimeUtc}",
                refreshTokenCreatedEvent.RefreshToken.Id,
                sourceIpAddress,
                DateTime.UtcNow);

            return;
        }

        await RevokeOldRefreshToken(user, sourceIpAddress);

        await UpdateUserWithNewRefreshToken(refreshTokenCreatedEvent, user, sourceIpAddress);
    }


    private async Task RevokeOldRefreshToken(User user, string? sourceIpAddress)
    {
        var userRefreshTokens = await _refreshTokenRepository.ListTokensByUserIdAsync(UserId.Create(Guid.Parse(user.Id.ToString()!)));

        if (userRefreshTokens.Count > 0)
        {
            var oldRefreshToken = await _refreshTokenRepository.GetById(
                RefreshTokenId.Create(Guid.Parse(user.RefreshTokenId!.ToString()!)));

            if (oldRefreshToken is not null)
            {
                oldRefreshToken.Revoke();
                await _refreshTokenRepository.UpdateTokenAsync(oldRefreshToken);
                _logger.LogInformation(
                    "User refresh token revoked successful, {@RefreshTokenId}, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
                    oldRefreshToken.Id,
                    user.Id,
                    sourceIpAddress,
                    DateTime.UtcNow);
            }
        }
    }


    private async Task UpdateUserWithNewRefreshToken(
        RefreshTokenCreated refreshTokenCreatedEvent,
        User user,
        string? sourceIpAddress)
    {
        user.UpdateRefreshToken(refreshTokenCreatedEvent.RefreshToken.Id);

        await _userRepository.UpdateAsync(user);

        _logger.LogInformation(
            "User refresh token updated successful, {@RefreshTokenId}, {@UserId}, {@SourceIpAddress}, {@DateTimeUtc}",
            refreshTokenCreatedEvent.RefreshToken.Id,
            user.Id,
            sourceIpAddress,
            DateTime.UtcNow);
    }
}
