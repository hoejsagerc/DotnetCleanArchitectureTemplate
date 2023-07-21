using ErrorOr;
using MediatR;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Application.Authentication.v1.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<RefreshToken>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<RefreshToken>> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(
            UserId.Create(Guid.Parse(command.UserId.ToString())));

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        var refreshToken = await _refreshTokenRepository.GetById(
            RefreshTokenId.Create(Guid.Parse(user.RefreshTokenId.ToString()!)));

        if (refreshToken is null)
        {
            return Errors.RefreshToken.NotFound;
        }

        refreshToken.Revoke();

        await _refreshTokenRepository.UpdateTokenAsync(refreshToken);

        return refreshToken;
    }
}
