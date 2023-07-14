using Pokemon.Application.Common.Interfaces.Services;

namespace Pokemon.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}