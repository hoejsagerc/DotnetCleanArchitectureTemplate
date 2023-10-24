using MicroServiceTemplatev1.Application.Common.Mappings;
using MicroServiceTemplatev1.Domain.Entities;

namespace MicroServiceTemplatev1.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; init; }

    public bool Done { get; init; }
}
