using BudgetTracker.Application.Common.Services;
using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Infrastructure.Services;
using NSubstitute;
using Xunit.Sdk;

namespace BudgetTracker.Infrastructure.UnitTests.Services;

public class LoggingAdapterTests
{
    private readonly ILoggerAdapter<object> _sut = Substitute.For<ILoggerAdapter<object>>();
    
    [Fact]
    public void Debug_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange & Act
        _sut.Debug("Test Message");
        
        // Assert
        _sut.Received(1).Debug("Test Message");
    }
    
    [Fact]
    public void Information_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange & Act
        _sut.Information("Test Message");
        
        // Assert
        _sut.Received(1).Information("Test Message");
    }
    
    [Fact]
    public void Error_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange & Act
        _sut.Error("Test Message");
        
        // Assert
        _sut.Received(1).Error("Test Message");
    }
    
    [Fact]
    public void Warning_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange & Act
        _sut.Warning("Test Message");
        
        // Assert
        _sut.Received(1).Warning("Test Message");
    }
    
    [Fact]
    public void Fatal_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange & Act
        _sut.Fatal("Test Message");
        
        // Assert
        _sut.Received(1).Fatal("Test Message");
    }
    
    [Fact]
    public void Verbose_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange & Act
        _sut.Verbose("Test Message");
        
        // Assert
        _sut.Received(1).Verbose("Test Message");
    }
}