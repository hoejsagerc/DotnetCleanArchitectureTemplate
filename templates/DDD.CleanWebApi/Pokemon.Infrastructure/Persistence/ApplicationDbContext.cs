using Microsoft.EntityFrameworkCore;
using Pokemon.Domain.Common.Models;
using Pokemon.Domain.PokemonAggregate;
using Pokemon.Domain.UserAggregate;
using Pokemon.Infrastructure.Persistence.Interceptors;

namespace Pokemon.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventInterceptor;
    private readonly UpdateAuditableEntitiesInterceptor _updateAuditableEntitiesInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventInterceptor,
        UpdateAuditableEntitiesInterceptor updateAuditableEntitiesInterceptor) : base(options)
    {
        _publishDomainEventInterceptor = publishDomainEventInterceptor;
        _updateAuditableEntitiesInterceptor = updateAuditableEntitiesInterceptor;
    }

    public DbSet<PocketMonster> Pokemons { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
                .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            _publishDomainEventInterceptor,
            _updateAuditableEntitiesInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}