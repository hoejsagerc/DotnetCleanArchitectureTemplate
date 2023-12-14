using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using ErrorOr;
using MediatR;

namespace BudgetTracker.Application.Trackable.v1.Commands.CreateTrackable;

public record CreateTrackableCommand(
    string Name,
    string CategoryId,
    TrackableType Type,
    decimal Amount,
    string Details,
    DateTime DateTime,
    string UserId) : IRequest<ErrorOr<TrackableAggregate>>;