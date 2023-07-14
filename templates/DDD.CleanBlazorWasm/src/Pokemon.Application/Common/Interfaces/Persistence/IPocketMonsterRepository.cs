using Pokemon.Domain.Common.PokemonAggregate.ValueObjects;
using Pokemon.Domain.PocketMonsterAggregate;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Application.Common.Interfaces.Persistence;

public interface IPocketMonsterRepository
{
    Task AddAsync(PocketMonster pokemon);
    Task<List<PocketMonster>> ListByUserIdAsync(UserId userId);
    Task<PocketMonster?> GetByIdAsync(PocketMonsterId pokemonId);
    Task UpdateAsync(PocketMonster pokemon);
    Task DeleteAsync(PocketMonster pokemon);
}