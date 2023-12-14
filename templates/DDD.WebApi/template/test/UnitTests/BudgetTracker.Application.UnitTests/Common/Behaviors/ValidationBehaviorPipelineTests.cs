using BudgetTracker.Application.Common.Behaviors;
using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace BudgetTracker.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorPipelineTests : IClassFixture<ValidationBehaviorTestsFixture>
{
    private readonly ValidationBehaviorTestsFixture _fixture;

    public ValidationBehaviorPipelineTests(ValidationBehaviorTestsFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task PipelineTestCommandHandler_WhenValidatorIsNull_ShouldProcessRequest()
    {
        // Arrange
        var sut = new ValidationBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>(
            _fixture.BehaviorLogger, _fixture.HttpContextAccessor, _fixture.DateTimeProvider);

        var command = new PipelineTestCommand("Success");

        // Act
        var result = await sut.Handle(command,
            async () => await Task.FromResult(new ErrorOr<PipelineCommandResponse>()), CancellationToken.None);


        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
    }


    [Fact]
    public async Task PipelineTestCommandHandler_WhenValidatorIsValid_ShouldProcessRequest()
    {
        // Arrange
        var validator = Substitute.For<IValidator<PipelineTestCommand>>();
        validator.ValidateAsync(Arg.Any<PipelineTestCommand>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var sut = new ValidationBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>(
            _fixture.BehaviorLogger, _fixture.HttpContextAccessor, _fixture.DateTimeProvider, validator);

        var command = new PipelineTestCommand("Success");

        // Act
        var result = await sut.Handle(command,
            async () => await Task.FromResult(new ErrorOr<PipelineCommandResponse>()), CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
    }


    [Fact]
    public async Task PipelineTestCommandHandler_WhenValidationFails_ShouldLogErrorsAndReturnErrorResponse()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("PropertyName1", "Error message 1"),
            new ValidationFailure("PropertyName2", "Error message 2")
        };
        var validationResult = new ValidationResult(validationFailures);
        
        var validator = Substitute.For<IValidator<PipelineTestCommand>>();
        validator.ValidateAsync(Arg.Any<PipelineTestCommand>(), 
            Arg.Any<CancellationToken>()).Returns(validationResult);

        var sut = new ValidationBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>(
            _fixture.BehaviorLogger, _fixture.HttpContextAccessor, _fixture.DateTimeProvider, validator);

        var command = new PipelineTestCommand("Error");

        // Act
        var result = await sut.Handle(command,
            async () => await Task.FromResult(new ErrorOr<PipelineCommandResponse>()), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ErrorOr<PipelineCommandResponse>>();
        
        _fixture.BehaviorLogger.Received(1).Error(
            Arg.Is<string>("Validation Error {@Error}, {@SourceIpAddress}, {@DateTimeUtc}"),
            Arg.Is(validationFailures[0]),
            Arg.Is<string>("n/a"),
            Arg.Is<DateTime>(_fixture.DateTimeProvider.UtcNow));
            
        _fixture.BehaviorLogger.Received(1).Error(
            Arg.Is<string>("Validation Error {@Error}, {@SourceIpAddress}, {@DateTimeUtc}"),
            Arg.Is(validationFailures[1]),
            Arg.Is<string>("n/a"),
            Arg.Is<DateTime>(_fixture.DateTimeProvider.UtcNow));
    }
}