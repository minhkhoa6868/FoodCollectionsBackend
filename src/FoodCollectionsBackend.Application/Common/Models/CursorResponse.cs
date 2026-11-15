using Microsoft.EntityFrameworkCore;

namespace FoodCollectionsBackend.Application.Common.Models;

public class CursorResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public string? NextCursor { get; set; }
    public bool HasNextPage => NextCursor != null;

    public static async Task<CursorResponse<T>> ToCursorResultAsync(
        IQueryable<T> query,
        int pageSize,
        Func<T, string> cursorSelector,  // select item from last cursor
        CancellationToken cancellationToken = default)
    {
        var items = await query
            .Take(pageSize + 1)  // take one extra item to check if there is a next page
            .ToListAsync(cancellationToken);

        var hasNextPage = items.Count > pageSize;
        if (hasNextPage) items.RemoveAt(items.Count - 1);

        return new CursorResponse<T>
        {
            Items = items,
            NextCursor = hasNextPage ? cursorSelector(items[^1]) : null
        };
    }
}
