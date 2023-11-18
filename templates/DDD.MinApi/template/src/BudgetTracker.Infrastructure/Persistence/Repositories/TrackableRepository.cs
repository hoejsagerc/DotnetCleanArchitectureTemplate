using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Domain.Common.Models;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Persistence.Repositories;

public class TrackableRepository : ITrackableRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TrackableRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TrackableAggregate> AddAsync(TrackableAggregate trackable, CancellationToken cancellationToken)
    {
        await _dbContext.Trackables.AddAsync(trackable, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return trackable;
    }

    public async Task UpdateAsync(TrackableAggregate trackable, CancellationToken cancellationToken)
    {
        _dbContext.Trackables.Update(trackable);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TrackableAggregate trackable, CancellationToken cancellationToken)
    {
        _dbContext.Trackables.Remove(trackable);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TrackableAggregate?> GetByIdAsync(TrackableId trackableId, CancellationToken cancellationToken)
    {
        var trackable = await _dbContext.Trackables
            .Where(t => t.Id == trackableId).SingleOrDefaultAsync(cancellationToken);

        return trackable ?? null;
    }


    public async Task<PagedList<TrackableAggregate>> GetAllAsync(string? userId, string? name, 
        int page, int pageSize, CancellationToken cancellationToken)
    {
        IQueryable<TrackableAggregate> query = _dbContext.Trackables;
        
        if (userId is not null)
        {
            query = query.Where(t => t.UserId == userId);
        }
        
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(t => t.Name.Contains(name));
        }
        
        var trackables = await query
            .OrderBy(t => t.CreatedOnUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        var totalCount = query.CountAsync(cancellationToken);
        
        var hasNextPage = page * pageSize < totalCount.Result;
        var hasPreviousPage = page * pageSize > 1;

        return new PagedList<TrackableAggregate>(trackables, page, pageSize, 
            totalCount.Result, hasNextPage, hasPreviousPage);
    }
}