using BudgetTracker.Domain.Common.Models.Identities;

namespace BudgetTracker.Domain.TrackableAggregate.ValueObjects;

public sealed class TrackableId : AggregateRootId<Guid>
{
    private TrackableId(Guid value) : base(value)
    {
    }

    public static TrackableId CreateUnique()
    {
        return new TrackableId(Guid.NewGuid());
    }

    public static TrackableId Create(Guid value)
    {
        return new TrackableId(value);
    }

    /// <summary>
    /// Method for creating a TrackableId <see cref="TrackableId"/> from a string
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns a TrackableId value object <see cref="TrackableId"/></returns>
    /// <exception cref="ArgumentException"></exception>
    public static TrackableId Create(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            throw new ArgumentException("Invalid TrackableId");
        }

        return new TrackableId(guid);
    }
}