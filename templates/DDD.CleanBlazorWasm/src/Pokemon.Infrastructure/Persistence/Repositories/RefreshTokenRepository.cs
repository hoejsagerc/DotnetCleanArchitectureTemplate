using Microsoft.EntityFrameworkCore;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _dbContext.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetById(RefreshTokenId refreshTokenId)
    {
        IQueryable<RefreshToken> query = _dbContext.RefreshTokens
            .Where(r => r.Id == refreshTokenId);

        return await query.SingleOrDefaultAsync();
    }

    public async Task<RefreshToken?> GetByUserIdAsync(UserId userId)
    {
        IQueryable<RefreshToken> query = _dbContext.RefreshTokens
            .Where(r => r.UserId == userId);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<RefreshToken>> ListTokensByUserIdAsync(UserId userId)
    {
        IQueryable<RefreshToken> query = _dbContext.RefreshTokens
            .Where(r => r.UserId == userId);

        return await query.ToListAsync();
    }

    public async Task UpdateTokenAsync(RefreshToken refreshToken)
    {
        _dbContext.Update(refreshToken);

        await _dbContext.SaveChangesAsync();
    }
}
