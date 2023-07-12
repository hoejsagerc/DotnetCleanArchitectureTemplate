using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.UserAggregate.Events;

public record UserCreated(User User) : IDomainEvent;