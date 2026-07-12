using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Application.Common.Models;

namespace FoodCollectionsBackend.Application.Common.Extentions;

public static class PaginationExtention
{
    public static async Task<PaginationResponse<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> query,
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)paginationRequest.PageSize);

        var items = await query
            .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginationResponse<T>
        {
            Items = items,
            CurrentPage = paginationRequest.Page,
            PageSize = paginationRequest.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
}
