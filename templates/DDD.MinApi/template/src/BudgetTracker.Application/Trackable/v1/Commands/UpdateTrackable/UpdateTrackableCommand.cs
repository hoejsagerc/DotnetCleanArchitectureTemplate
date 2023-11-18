using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Commands.UpdateTrackable;

public record UpdateTrackableCommand(string Name, string Id,
    string CategoryId, TrackableType Type, decimal Amount, string Details, string UserId, DateTime DateTime) 
    : IRequest<ErrorOr<TrackableAggregate>>;