using Pokemon.Domain.UserAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(UserId userId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}