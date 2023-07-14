using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.PocketMonsterAggregate.Events;

public record PocketMonsterUpdated(PocketMonster Pokemon) : IDomainEvent;