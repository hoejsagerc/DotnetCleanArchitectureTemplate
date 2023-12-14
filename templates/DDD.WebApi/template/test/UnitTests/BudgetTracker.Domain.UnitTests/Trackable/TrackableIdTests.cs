using BudgetTracker.Domain.TrackableAggregate.ValueObjects;
using FluentAssertions;

namespace BudgetTracker.Domain.UnitTests.Trackable;

public class TrackableIdTests
{
    [Fact]
    public void TrackableId_CreateUnique_ShouldCreateUniqueId()
    {
        // Arrange
        var id1 = TrackableId.CreateUnique();
        var id2 = TrackableId.CreateUnique();

        // Assert
        id1.Should().NotBe(id2);
    }

    [Fact]
    public void TrackableId_Create_ShouldCreateIdWithSpecifiedGuid()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        var id = TrackableId.Create(guid);

        // Assert
        id.Value.Should().Be(guid);
    }

    [Fact]
    public void TrackableId_Create_WithValidGuidString_ShouldCreateId()
    {
        // Arrange
        string validGuidString = Guid.NewGuid().ToString();

        // Act
        var id = TrackableId.Create(validGuidString);

        // Assert
        id.Value.ToString().Should().Be(validGuidString);
    }

    [Fact]
    public void TrackableId_Create_WithInvalidGuidString_ShouldThrowArgumentException()
    {
        // Arrange
        string invalidGuidString = "invalid-guid";

        // Assert
        Action action = () => TrackableId.Create(invalidGuidString);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid TrackableId");
    }
}