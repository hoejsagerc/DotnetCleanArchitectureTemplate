using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.PocketMonsterAggregate.Events;

public record PocketMonsterTransfered(PocketMonster Pokemon) : IDomainEvent;