using BudgetTracker.Application.Common.Persistence.Repositories;
using BudgetTracker.Application.Common.Services;
using BudgetTracker.Domain.TrackableAggregate;
using ErrorOr;
using MediatR;

namespace BudgetTracker.Application.Trackable.v1.Commands.CreateTrackable;

public class CreateTrackableCommandHandler : IRequestHandler<CreateTrackableCommand, ErrorOr<TrackableAggregate>>
{
    private readonly ILoggerAdapter<CreateTrackableCommandHandler> _logger;
    private readonly ITrackableRepository _trackableRepository;
public CreateTrackableCommandHandler(ILoggerAdapter<CreateTrackableCommandHandler> logger, 
        ITrackableRepository trackableRepository)
    {
        _logger = logger;
        _trackableRepository = trackableRepository;
    }

    public async Task<ErrorOr<TrackableAggregate>> Handle(CreateTrackableCommand command, 
        CancellationToken cancellationToken)
    {
        var trackable = TrackableAggregate.Create(
            command.Name, command.CategoryId, command.Type, command.Amount, command.Details, command.UserId);
        
        var result = await _trackableRepository.AddAsync(trackable, cancellationToken);

        return result;
    }
}
