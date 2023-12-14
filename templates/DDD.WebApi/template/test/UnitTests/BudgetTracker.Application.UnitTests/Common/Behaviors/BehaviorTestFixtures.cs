using BudgetTracker.Application.Common.Behaviors;
using BudgetTracker.Application.Common.Services;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace BudgetTracker.Application.UnitTests.Common.Behaviors;

public record PipelineCommandResponse(string Message);
public record PipelineTestCommand(string Message) : IRequest<ErrorOr<PipelineCommandResponse>>;

public class PipelineTestCommandHandler : IRequestHandler<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>
{
    public async Task<ErrorOr<PipelineCommandResponse>> Handle(PipelineTestCommand command, 
        CancellationToken cancellationToken)
    {
        if (command.Message == "Error")
        {
            return Error.Validation(nameof(command.Message), "Error");
        }

        return await Task.FromResult(new PipelineCommandResponse(command.Message));
    }
}

public class ValidationBehaviorTestsFixture
{
    public readonly IHttpContextAccessor HttpContextAccessor;
    public ILoggerAdapter<ValidationBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>>
        BehaviorLogger;
    public readonly ILoggerAdapter<PipelineTestCommandHandler> CommandLogger;
    public readonly IDateTimeProvider DateTimeProvider;

    public ValidationBehaviorTestsFixture()
    {
        HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
        HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext());
        BehaviorLogger = Substitute
            .For<ILoggerAdapter<ValidationBehaviorPipeline<PipelineTestCommand,
                ErrorOr<PipelineCommandResponse>>>>();
        CommandLogger = Substitute.For<ILoggerAdapter<PipelineTestCommandHandler>>();
        DateTimeProvider = Substitute.For<IDateTimeProvider>();
        DateTimeProvider.UtcNow
            .Returns(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }
    
    public void ResetLogger()
    {
        BehaviorLogger = Substitute
            .For<ILoggerAdapter<ValidationBehaviorPipeline<PipelineTestCommand,
                ErrorOr<PipelineCommandResponse>>>>();
    }
}

public class LoggingBehaviorTestsFixture
{
    public readonly IHttpContextAccessor HttpContextAccessor;
    public ILoggerAdapter<LoggingBehaviorPipeline<PipelineTestCommand, ErrorOr<PipelineCommandResponse>>>
        BehaviorLogger;
    public readonly ILoggerAdapter<PipelineTestCommandHandler> CommandLogger;
    public readonly IDateTimeProvider DateTimeProvider;

    public LoggingBehaviorTestsFixture()
    {
        HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
        HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext());
        BehaviorLogger = Substitute
            .For<ILoggerAdapter<LoggingBehaviorPipeline<PipelineTestCommand,
                ErrorOr<PipelineCommandResponse>>>>();
        CommandLogger = Substitute.For<ILoggerAdapter<PipelineTestCommandHandler>>();
        DateTimeProvider = Substitute.For<IDateTimeProvider>();
        DateTimeProvider.UtcNow
            .Returns(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }
    
    public void ResetLogger()
    {
        BehaviorLogger = Substitute
            .For<ILoggerAdapter<LoggingBehaviorPipeline<PipelineTestCommand,
                ErrorOr<PipelineCommandResponse>>>>();
    }
}