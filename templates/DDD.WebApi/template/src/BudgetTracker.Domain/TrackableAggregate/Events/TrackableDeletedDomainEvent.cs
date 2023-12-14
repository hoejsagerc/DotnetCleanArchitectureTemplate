using BudgetTracker.Domain.Common.Models;

namespace BudgetTracker.Domain.TrackableAggregate.Events;

public sealed record TrackableDeletedDomainEvent(TrackableAggregate Trackable) : IDomainEvent;