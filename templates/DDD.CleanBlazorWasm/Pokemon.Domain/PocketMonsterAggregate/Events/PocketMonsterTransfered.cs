using Pokemon.Domain.Common.Models;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Domain.PocketMonsterAggregate.Events;

public record PocketMonsterTransfered(PocketMonster Pokemon, UserId OldUserId) : IDomainEvent;