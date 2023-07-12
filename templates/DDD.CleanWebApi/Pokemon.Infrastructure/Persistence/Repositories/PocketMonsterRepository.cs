using Microsoft.EntityFrameworkCore;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PokemonAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Repositories;

public class PocketMonsterRepository : IPocketMonsterRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PocketMonsterRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(PocketMonster pokemon)
    {
        _dbContext.Add(pokemon);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(PocketMonster pokemon)
    {
        _dbContext.Remove(pokemon);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<PocketMonster?> GetByIdAsync(PokemonId pokemonId)
    {
        IQueryable<PocketMonster> query = _dbContext.Pokemons
            .Where(p => p.Id == pokemonId);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<PocketMonster>> ListByUserIdAsync(UserId userId)
    {
        IQueryable<PocketMonster> query = _dbContext.Pokemons
            .Where(p => p.UserId! == userId);

        return await query.ToListAsync();
    }

    public async Task UpdateAsync(PocketMonster pokemon)
    {
        _dbContext.Update(pokemon);

        await _dbContext.SaveChangesAsync();
    }
}
