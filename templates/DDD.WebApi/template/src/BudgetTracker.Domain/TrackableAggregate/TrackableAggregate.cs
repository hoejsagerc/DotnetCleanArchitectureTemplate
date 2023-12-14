using BudgetTracker.Domain.Common.Models;
using BudgetTracker.Domain.TrackableAggregate.Enums;
using BudgetTracker.Domain.TrackableAggregate.Events;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;

namespace BudgetTracker.Domain.TrackableAggregate;

public sealed class TrackableAggregate : AggregateRoot<TrackableId, Guid>, IAuditableEntity
{
    public string Name { get; private set; } 
    public string CategoryId { get; private set; }
    public TrackableType Type { get; private set; }
    public decimal Amount { get; private set; } 
    public string Details { get; private set; }
    public string UserId { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }

    
    #pragma warning disable CS8618
    private TrackableAggregate()
    {
    }
    #pragma warning restore CS8618


    private TrackableAggregate(TrackableId trackableId, string name, string categoryId, TrackableType type, decimal amount,
        string details, string userId) : base(trackableId)
    {
        Name = name;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Details = details;
        UserId = userId;
    }

    
    /// <summary>
    /// Method for creating a new Trackable <see cref="TrackableAggregate"/>
    /// <remarks>This method will generate a domain event <see cref="TrackableCreatedDomainEvent"/></remarks>
    /// </summary>
    /// <param name="name">The name of the trackable</param>
    /// <param name="categoryId">The category of the trackable<see cref="CategoryId"/></param>
    /// <param name="type">The type of the trackable<see cref="TrackableType"/></param>
    /// <param name="amount">The amount of the trackable</param>
    /// <param name="details">A description of the trackable</param>
    /// <param name="userId">The user id for the trackable <see cref="UserId"/></param>
    /// <returns>Trackable aggregate <see cref="TrackableAggregate"/></returns>
    public static TrackableAggregate Create(string name, string categoryId, TrackableType type, decimal amount,
        string details, string userId)
    {
        var trackable = new TrackableAggregate(TrackableId.CreateUnique(), name, categoryId, type, amount,
            details, userId);
        
        trackable.AddDomainEvent(new TrackableCreatedDomainEvent(trackable));

        return trackable;
    }


    /// <summary>
    /// Method for updating a Trackable <see cref="TrackableAggregate"/>
    /// <remarks>This method will generate a domain event <see cref="TrackableUpdatedDomainEvent"/></remarks>
    /// </summary>
    /// <param name="name">The name of the trackable</param>
    /// <param name="categoryId">The category of a trackable <see cref="CategoryId"/></param>
    /// <param name="type">The type of the trackable <see cref="TrackableType"/></param>
    /// <param name="amount">The amount of the trackable</param>
    /// <param name="details">A description of the trackable</param>
    /// <param name="userId">The user id for the trackable <see cref="UserId"/></param>
    public void Update(string name, string categoryId, TrackableType type, decimal amount,
        string details, string userId)
    {
        Name = name;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Details = details;
        UserId = userId;
    }

    
    /// <summary>
    /// Method for deleting a Trackable
    /// This method will generate a domain event <see cref="TrackableDeletedDomainEvent"/>
    /// </summary>
    public void Delete()
    {
        this.AddDomainEvent(new TrackableDeletedDomainEvent(this));
    }
}