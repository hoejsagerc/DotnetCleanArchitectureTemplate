using MicroServiceTemplatev1.Application.Common.Mappings;
using MicroServiceTemplatev1.Domain.Entities;

namespace MicroServiceTemplatev1.Application.Common.Models;

// Note: This is currently just used to demonstrate applying multiple IMapFrom attributes.
public class LookupDto : IMapFrom<TodoList>, IMapFrom<TodoItem>
{
    public int Id { get; init; }

    public string? Title { get; init; }
}
