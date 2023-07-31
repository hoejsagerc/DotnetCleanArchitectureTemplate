using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PocketMonsterAggregate;

namespace Pokemon.Application.Common.Interfaces.Persistence;

public interface IPocketMonsterRepository
{
    Task AddAsync(PocketMonster pokemon);
    Task<PocketMonster?> GetByIdAsync(PocketMonsterId pokemonId);
    Task UpdateAsync(PocketMonster pokemon);
    Task DeleteAsync(PocketMonster pokemon);
}