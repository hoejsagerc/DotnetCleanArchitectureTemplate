using Microsoft.EntityFrameworkCore;
using Pokemon.Application.Common.Interfaces.Persistence;
using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PocketMonsterAggregate;

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

    public async Task<PocketMonster?> GetByIdAsync(PocketMonsterId pokemonId)
    {
        IQueryable<PocketMonster> query = _dbContext.Pokemons
            .Where(p => p.Id == pokemonId);

        return await query.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(PocketMonster pokemon)
    {
        _dbContext.Update(pokemon);

        await _dbContext.SaveChangesAsync();
    }
}
