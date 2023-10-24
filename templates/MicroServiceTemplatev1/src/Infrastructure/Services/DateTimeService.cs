using MicroServiceTemplatev1.Application.Common.Interfaces;

namespace MicroServiceTemplatev1.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
