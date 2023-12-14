using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Domain.Common.Models;
using BudgetTracker.Domain.TrackableAggregate;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Queries.GetTrackables;

public class GetTrackablesQueryHandler : IRequestHandler<GetTrackablesQuery, ErrorOr<PagedList<TrackableAggregate>>>
{
    private readonly ITrackableRepository _trackableRepository;

    public GetTrackablesQueryHandler(ITrackableRepository trackableRepository)
    {
        _trackableRepository = trackableRepository;
    }

    public async Task<ErrorOr<PagedList<TrackableAggregate>>> Handle(GetTrackablesQuery query, 
        CancellationToken cancellationToken)
    {
        var trackables = await _trackableRepository
            .GetAllAsync(query.UserId, query.Name, query.Page, query.PageSize, cancellationToken);

        return trackables;
    }
}