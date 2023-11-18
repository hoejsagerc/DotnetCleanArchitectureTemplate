using FluentValidation;

namespace BudgetTracker.Application.Trackable.v1.Queries.GetTrackableById;

public class GetTrackableByIdValidator : AbstractValidator<GetTrackableByIdQuery>
{
    public GetTrackableByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .Must(BeValidGuid)
            .WithMessage("Id must be a valid guid");
    }

    private bool BeValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}