namespace BudgetTracker.Contracts.v1.Common;

public record PagedListResponse<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    bool HasNextPage,
    bool HasPreviousPage);