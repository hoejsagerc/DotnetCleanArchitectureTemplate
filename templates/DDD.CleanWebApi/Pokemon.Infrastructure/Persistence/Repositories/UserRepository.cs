using Microsoft.EntityFrameworkCore;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.UserAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(UserId userId)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Update(user);

        await _dbContext.SaveChangesAsync();
    }
}
