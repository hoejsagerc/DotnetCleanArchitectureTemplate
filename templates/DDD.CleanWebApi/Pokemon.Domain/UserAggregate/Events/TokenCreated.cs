using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.UserAggregate.Events;

public record RefreshTokenCreated(RefreshToken RefreshToken) : IDomainEvent;