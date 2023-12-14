using BudgetTracker.Application.Common.Persistence;
using BudgetTracker.Domain.Common.Models;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
    private readonly UpdateAuditableEntitiesInterceptor _updateAuditableEntitiesInterceptor;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor,
        UpdateAuditableEntitiesInterceptor updateAuditableEntitiesInterceptor, 
        IDbConnectionFactory dbConnectionFactory) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
        _updateAuditableEntitiesInterceptor = updateAuditableEntitiesInterceptor;
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public DbSet<TrackableAggregate> Trackables { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(
                _publishDomainEventsInterceptor,
                _updateAuditableEntitiesInterceptor);
        
        base.OnConfiguring(optionsBuilder);
    }
}