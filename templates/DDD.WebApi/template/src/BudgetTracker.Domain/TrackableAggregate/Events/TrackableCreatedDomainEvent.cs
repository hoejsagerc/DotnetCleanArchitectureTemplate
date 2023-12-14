using BudgetTracker.Domain.Common.Models;

namespace BudgetTracker.Domain.TrackableAggregate.Events;

public sealed record TrackableCreatedDomainEvent(TrackableAggregate Trackable) : IDomainEvent;