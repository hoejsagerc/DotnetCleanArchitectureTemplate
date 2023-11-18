using BudgetTracker.Domain.TrackableAggregate;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Application.Common.Persistence;

public interface IApplicationDbContext
{
    DbSet<TrackableAggregate> Trackables { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}