using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.PokemonAggregate.Events;

public record PokemonUpdated(PocketMonster Pokemon) : IDomainEvent;