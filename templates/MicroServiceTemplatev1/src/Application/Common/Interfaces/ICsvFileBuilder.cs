using MicroServiceTemplatev1.Application.TodoLists.Queries.ExportTodos;

namespace MicroServiceTemplatev1.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
