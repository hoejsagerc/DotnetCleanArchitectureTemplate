namespace BudgetTracker.Contracts.v1.Trackable;

public record CreateTrackableRequest(string Name, string CategoryId, TrackableTypeResponse
    Type, decimal Amount, string Details, string UserId);