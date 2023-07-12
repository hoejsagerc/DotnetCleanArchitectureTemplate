using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PokemonAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Application.Common.Interfaces.Persistence;

public interface IPocketMonsterRepository
{
    Task AddAsync(PocketMonster pokemon);
    Task<List<PocketMonster>> ListByUserIdAsync(UserId userId);
    Task<PocketMonster?> GetByIdAsync(PokemonId pokemonId);
    Task UpdateAsync(PocketMonster pokemon);
    Task DeleteAsync(PocketMonster pokemon);
}