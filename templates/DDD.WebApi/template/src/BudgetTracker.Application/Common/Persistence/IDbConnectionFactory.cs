using System.Data;

namespace BudgetTracker.Application.Common.Persistence;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}