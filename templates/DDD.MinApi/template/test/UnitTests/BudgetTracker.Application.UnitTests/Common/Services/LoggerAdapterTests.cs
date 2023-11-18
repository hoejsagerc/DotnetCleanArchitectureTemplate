using BudgetTracker.Application.Common.Services;
using NSubstitute;

namespace BudgetTracker.Application.UnitTests.Common.Services;

public class LoggerAdapterTests
{
    private readonly ILoggerAdapter<object> _sut = Substitute.For<ILoggerAdapter<object>>();

    [Fact]
    public void Debug_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange && Act
        _sut.Debug("Test message");
        
        // Assert
        _sut.Received(1).Debug("Test message");
    }
    
    
    [Fact]
    public void Information_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange && Act
        _sut.Information("Test message");
        
        // Assert
        _sut.Received(1).Information("Test message");
    }

    [Fact]
    public void Error_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange && Act
        _sut.Error("Test message");
        
        // Assert
        _sut.Received(1).Error("Test message");
    }
    
    [Fact]
    public void Warning_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange && Act
        _sut.Warning("Test message");
        
        // Assert
        _sut.Received(1).Warning("Test message");
    }
    
    [Fact]
    public void Fatal_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange && Act
        _sut.Fatal("Test message");
        
        // Assert
        _sut.Received(1).Fatal("Test message");
    }
    
    [Fact]
    public void Verbose_CallsLoggerWithCorrectLevelAndMessage()
    {
        // Arrange && Act
        _sut.Verbose("Test message");
        
        // Assert
        _sut.Received(1).Verbose("Test message");
    }
}