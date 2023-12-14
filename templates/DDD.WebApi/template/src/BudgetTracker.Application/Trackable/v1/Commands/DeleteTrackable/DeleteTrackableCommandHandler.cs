using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Application.Common.Services;
using BudgetTracker.Domain.Common.DomainErrors;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Commands.DeleteTrackable;

public class DeleteTrackableCommandHandler : IRequestHandler<DeleteTrackableCommand, ErrorOr<TrackableAggregate>>
{
    private readonly ILoggerAdapter<DeleteTrackableCommandHandler> _logger;
    private readonly ITrackableRepository _trackableRepository;

    public DeleteTrackableCommandHandler(ILoggerAdapter<DeleteTrackableCommandHandler> logger, 
        ITrackableRepository trackableRepository)
    {
        _logger = logger;
        _trackableRepository = trackableRepository;
    }

    public async Task<ErrorOr<TrackableAggregate>> Handle(DeleteTrackableCommand command, 
        CancellationToken cancellationToken)
    {
        var trackable = await _trackableRepository.GetByIdAsync(TrackableId.Create(Guid.Parse(command.Id)), 
            cancellationToken);
        
        if (trackable is null)
        {
            _logger.Error("Trackable with id {TrackableId} was not found", command.Id);
            return Errors.Trackable.NotFound;
        }

        await _trackableRepository.DeleteAsync(trackable, cancellationToken);
        return trackable;
    }
}