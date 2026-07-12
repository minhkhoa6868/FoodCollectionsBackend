using System.Linq.Dynamic.Core;
namespace FoodCollectionsBackend.Application.Common.Extentions;

public static class QueryableExtension
{
    public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string? sortBy,
            string? sortDirection,
            string defaultSort,
            params string[] allowedFields)
    {
        var direction = sortDirection?.ToLower() == "desc"
            ? "descending"
            : "ascending";

        sortBy = string.IsNullOrWhiteSpace(sortBy)
            ? defaultSort
            : sortBy;

        if (!allowedFields.Contains(sortBy))
        {
            sortBy = defaultSort;
        }

        return query.OrderBy($"{sortBy} {direction}");
    }
}
