using BudgetTracker.Domain.TrackableAggregate;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Commands.DeleteTrackable;

public record DeleteTrackableCommand(string Id) : IRequest<ErrorOr<TrackableAggregate>>;