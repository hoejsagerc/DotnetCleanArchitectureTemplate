using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.AuthenticationAggregates.Events;

public record RefreshTokenCreated(RefreshToken RefreshToken) : IDomainEvent;