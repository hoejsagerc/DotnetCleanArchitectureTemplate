using BudgetTracker.Domain.Common.Models;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;

namespace BudgetTracker.Application.Common.Persistence.Repositories;

public interface ITrackableRepository
{
    Task<TrackableAggregate> AddAsync(TrackableAggregate trackable, CancellationToken cancellationToken);
    Task UpdateAsync(TrackableAggregate trackable, CancellationToken cancellationToken);
    Task DeleteAsync(TrackableAggregate trackable, CancellationToken cancellationToken);
    Task<TrackableAggregate?> GetByIdAsync(TrackableId trackableId, CancellationToken cancellationToken);
    Task<PagedList<TrackableAggregate>> GetAllAsync(string? userId, string? name, 
        int page, int pageSize, CancellationToken cancellationToken);
}