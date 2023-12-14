using BudgetTracker.Application.Common.Behaviors;
using ErrorOr;
using NSubstitute;

namespace BudgetTracker.Application.UnitTests.Common.Behaviors;

public class LoggingBehaviorPipelineTests : IClassFixture<LoggingBehaviorTestsFixture>
{
    private readonly LoggingBehaviorTestsFixture _fixture;

    public LoggingBehaviorPipelineTests(LoggingBehaviorTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PipelineTestCommandHandler_WhenValidCommand_ShouldLogInformation()
    {
        // Arrange
        _fixture.ResetLogger();
        var sut = new LoggingBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>(
            _fixture.BehaviorLogger, _fixture.HttpContextAccessor, _fixture.DateTimeProvider);
        var handler = new PipelineTestCommandHandler();
        var command = new PipelineTestCommand("Success");

        // Act
        await sut.Handle(command, 
            async () => await handler.Handle(command, CancellationToken.None),
            CancellationToken.None);

        // Assert
        _fixture.BehaviorLogger.Received(2).Information(Arg.Any<string>(), Arg.Any<object[]>());
        
        _fixture.BehaviorLogger.Received(1).Information(
            Arg.Is<string>(s => s.Contains("Starting request")),
            Arg.Is<string>(nameof(PipelineTestCommand)),
            Arg.Is<string>("n/a"),
            Arg.Is(_fixture.DateTimeProvider.UtcNow));
        
        _fixture.BehaviorLogger.Received(1).Information(
            Arg.Is<string>(s => s.Contains("Completed request")),
            Arg.Is<string>(nameof(PipelineTestCommand)),
            Arg.Is<string>("n/a"),
            Arg.Is(_fixture.DateTimeProvider.UtcNow));
    }
    
    [Fact]
    public async Task PipelineTestCommandHandler_WhenInValidCommand_ShouldLogErrors()
    {
        // Arrange
        _fixture.ResetLogger();
        var sut = new LoggingBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>(
            _fixture.BehaviorLogger, _fixture.HttpContextAccessor, _fixture.DateTimeProvider);
        var handler = new PipelineTestCommandHandler();
        var command = new PipelineTestCommand("Error");

        // Act
        await sut.Handle(command, 
            async () => await handler.Handle(command, CancellationToken.None),
            CancellationToken.None);

        // Assert
        _fixture.BehaviorLogger.Received(2).Information(Arg.Any<string>(), Arg.Any<object[]>());
        
        _fixture.BehaviorLogger.Received(1).Information(
            Arg.Is<string>(s => s.Contains("Starting request")),
            Arg.Is<string>(nameof(PipelineTestCommand)),
            Arg.Is<string>("n/a"),
            Arg.Is(_fixture.DateTimeProvider.UtcNow));
        
        _fixture.BehaviorLogger.Received(1).Error(
            Arg.Is<string>(s => s.Contains("Request error")),
            Arg.Is<string>(nameof(PipelineTestCommand)),
            Arg.Is<string>("n/a"),
            Arg.Is(_fixture.DateTimeProvider.UtcNow),
            Arg.Any<Error>());

        _fixture.BehaviorLogger.Received(1).Information(
            Arg.Is<string>(s => s.Contains("Completed request with errors")),
            Arg.Is<string>(nameof(PipelineTestCommand)),
            Arg.Is<string>("n/a"),
            Arg.Is(_fixture.DateTimeProvider.UtcNow));
    }
}