using Microsoft.EntityFrameworkCore;

namespace MotoVerse.Core.Wrappers;

public class PaginatedResult<T>
{
    #region Fields
    public List<T> Data { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public object Meta { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public List<string> Messages { get; set; } = new();

    public bool Succeeded { get; set; }
    #endregion

    #region CTOR
    public PaginatedResult() { }
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    internal PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Succeeded = succeeded;
        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    #endregion

    #region Handler Methods
    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
    {
        return new(true, data, null, count, page, pageSize);
    }

    #endregion
}
public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        where T : class
    {
        if (source == null)
        {
            throw new Exception("Empty");
        }

        pageNumber = pageNumber == 0 ? 1 : pageNumber;

        pageSize = pageSize == 0 ? 10 : pageSize;

        int count = await source.AsNoTracking().CountAsync();

        if (count == 0)
            return PaginatedResult<T>.Success(new List<T>(), count, pageNumber, pageSize);

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var items =
            await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();

        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
    }
}