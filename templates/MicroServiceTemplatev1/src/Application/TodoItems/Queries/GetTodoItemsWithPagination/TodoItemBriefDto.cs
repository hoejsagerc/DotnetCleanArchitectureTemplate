using MicroServiceTemplatev1.Application.Common.Mappings;
using MicroServiceTemplatev1.Domain.Entities;

namespace MicroServiceTemplatev1.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto : IMapFrom<TodoItem>
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}
