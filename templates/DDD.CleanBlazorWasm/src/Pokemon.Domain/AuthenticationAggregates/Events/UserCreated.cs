using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.AuthenticationAggregates.Events;

public record UserCreated(User User) : IDomainEvent;