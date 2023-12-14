using BudgetTracker.Domain.TrackableAggregate;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Queries.GetTrackableById;

public record GetTrackableByIdQuery(string Id) : IRequest<ErrorOr<TrackableAggregate>>;