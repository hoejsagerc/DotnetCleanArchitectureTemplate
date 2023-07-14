using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Application.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetById(RefreshTokenId refreshTokenId);
    Task<List<RefreshToken>> ListTokensByUserIdAsync(UserId userId);
    Task<RefreshToken?> GetByUserIdAsync(UserId userId);
    Task UpdateTokenAsync(RefreshToken refreshToken);
    Task AddAsync(RefreshToken refreshToken);
}