using BudgetTracker.Domain.TrackableAggregate.Enums;
using BudgetTracker.Domain.TrackableAggregate.Events;
using BudgetTracker.Domain.UnitTests.DomainArchitecture;
using FluentAssertions;

namespace BudgetTracker.Domain.UnitTests.Trackable;

public class TrackableAggregateTests : BaseTest
{
    [Fact]
    public void Create_ShouldCreateTrackableWithCorrectProperties()
    {
        // Arrange
        var name = "Sample Trackable";
        var categoryId = "Category1";
        var type = TrackableType.Expense;
        var amount = 100.0m;
        var details = "Sample details";
        var dateTime = DateTime.UtcNow;
        var userId = "user123";

        // Act
        var trackable = TrackableAggregate.TrackableAggregate
            .Create(name, categoryId, type, amount, details, userId);

        // Assert
        trackable.Id.Should().NotBeNull();
        trackable.Name.Should().Be(name);
        trackable.CategoryId.Should().Be(categoryId);
        trackable.Type.Should().Be(type);
        trackable.Amount.Should().Be(amount);
        trackable.Details.Should().Be(details);
        trackable.UserId.Should().Be(userId);
    }

    [Fact]
    public void Update_ShouldUpdateTrackableProperties()
    {
        // Arrange
        var trackable = TrackableAggregate.TrackableAggregate
            .Create( "Initial Name",
                "Category1",
                TrackableType.Expense,
                50.0m,
                "Initial details",
                "user123");
        
        var updatedName = "Updated Name";
        var updatedCategoryId = "Category2";
        var updatedType = TrackableType.Expense;
        var updatedAmount = 75.0m;
        var updatedDetails = "Updated details";
        var updatedDateTime = DateTime.UtcNow.AddMinutes(30);
        var updatedUserId = "user456";

        // Act
        trackable.Update(updatedName, updatedCategoryId, updatedType, 
            updatedAmount, updatedDetails, updatedUserId);

        // Assert
        trackable.Name.Should().Be(updatedName);
        trackable.CategoryId.Should().Be(updatedCategoryId);
        trackable.Type.Should().Be(updatedType);
        trackable.Amount.Should().Be(updatedAmount);
        trackable.Details.Should().Be(updatedDetails);
        trackable.UserId.Should().Be(updatedUserId);
    }

    [Fact]
    public void Create_ShouldGenerateTrackableDeletedDomainEvent()
    {
        // Arrange & Act
        var trackable = TrackableAggregate.TrackableAggregate
            .Create("Sample Trackable",
                "Category1",
                TrackableType.Expense,
                100.0m,
                "Sample details",
                "user123");

        // Assert
        trackable.DomainEvents.Should().ContainSingle(e => e is TrackableCreatedDomainEvent);
    }
    

    [Fact]
    public void Delete_ShouldGenerateTrackableDeletedDomainEvent()
    {
        // Arrange
        var trackable = TrackableAggregate.TrackableAggregate
            .Create( "Sample Trackable",
                "Category1",
                TrackableType.Expense,
                100.0m, 
                "Sample details", 
                "user123");

        // Act
        trackable.Delete();

        // Assert
        trackable.DomainEvents.Should().ContainSingle(e => e is TrackableDeletedDomainEvent);
    }
}