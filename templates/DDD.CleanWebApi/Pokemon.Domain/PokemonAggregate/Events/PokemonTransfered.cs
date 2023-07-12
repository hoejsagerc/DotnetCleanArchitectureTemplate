using Pokemon.Domain.Common.Models;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Domain.PokemonAggregate.Events;

public record PokemonTransfered(PocketMonster Pokemon, UserId OldUserId) : IDomainEvent;