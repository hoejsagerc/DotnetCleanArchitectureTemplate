using System.Globalization;
using MicroServiceTemplatev1.Application.Common.Interfaces;
using MicroServiceTemplatev1.Application.TodoLists.Queries.ExportTodos;
using MicroServiceTemplatev1.Infrastructure.Files.Maps;
using CsvHelper;

namespace MicroServiceTemplatev1.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
