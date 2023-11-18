namespace BudgetTracker.Domain.Common.Models;

public class PagedList<T>
{
    public PagedList(List<T> items, int page, int pageSize, int totalCount, bool hasNextPage, bool hasPreviousPage)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }
}