using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Application.Common.Services;
using BudgetTracker.Application.Trackable.v1.Commands.DeleteTrackable;
using BudgetTracker.Domain.Common.DomainErrors;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;
using MediatR;
using ErrorOr;

namespace BudgetTracker.Application.Trackable.v1.Commands.UpdateTrackable;

public class UpdateTrackableCommandHandler : IRequestHandler<UpdateTrackableCommand, ErrorOr<TrackableAggregate>>
{
    private readonly ILoggerAdapter<DeleteTrackableCommandHandler> _logger;
    private readonly ITrackableRepository _trackableRepository;
    public UpdateTrackableCommandHandler(ILoggerAdapter<DeleteTrackableCommandHandler> logger, 
        ITrackableRepository trackableRepository)
    {
        _logger = logger;
        _trackableRepository = trackableRepository;
    }

        
    public async Task<ErrorOr<TrackableAggregate>> Handle(UpdateTrackableCommand command, 
        CancellationToken cancellationToken)
    {
        var trackable = await _trackableRepository.GetByIdAsync(
            TrackableId.Create(Guid.Parse(command.Id)), cancellationToken);

        if (trackable is null)
        {
            _logger.Error("Trackable with {TrackableId} was not found", command.Id);
            return Errors.Trackable.NotFound;
        }
        
        trackable.Update(command.Name, command.CategoryId, command.Type, 
            command.Amount, command.Details, command.UserId);
        
        await _trackableRepository.UpdateAsync(trackable, cancellationToken);
        _logger.Information("Updated {@Trackable} with {TrackableId}", trackable, command.Id);
        return trackable;
    }
}