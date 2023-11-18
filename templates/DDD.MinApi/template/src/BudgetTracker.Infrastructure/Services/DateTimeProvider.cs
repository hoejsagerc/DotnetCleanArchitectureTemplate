using BudgetTracker.Application.Common.Services;

namespace BudgetTracker.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}