using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Application.Common.Services;
using BudgetTracker.Domain.Common.DomainErrors;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Queries.GetTrackableById;

public class GetTrackableByIdQueryHandler : IRequestHandler<GetTrackableByIdQuery, ErrorOr<TrackableAggregate>>
{
    private readonly ILoggerAdapter<GetTrackableByIdQueryHandler> _logger;
    private readonly ITrackableRepository _trackableRepository;

    public GetTrackableByIdQueryHandler(ILoggerAdapter<GetTrackableByIdQueryHandler> logger, 
        ITrackableRepository trackableRepository)
    {
        _logger = logger;
        _trackableRepository = trackableRepository;
    }

    public async Task<ErrorOr<TrackableAggregate>> Handle(GetTrackableByIdQuery query, 
        CancellationToken cancellationToken)
    {
        var trackable = await _trackableRepository.GetByIdAsync(
            TrackableId.Create(Guid.Parse(query.Id)), cancellationToken);

        if (trackable is null)
        {
            _logger.Error("Trackable with {TrackableId} was not found", query.Id);
            return Errors.Trackable.NotFound;
        }

        return trackable;
    }
}