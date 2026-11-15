using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces.FoodRecords;
using FoodCollectionsBackend.Application.Features.FoodRecords.Queries;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;
namespace FoodCollectionsBackend.Infrastructure.Services.FoodRecords;

public class FoodRecordQueryService(
    IAppDbContext context,
    ICurrentUser currentUser,
    ICacheService cacheService) : IFoodRecordQueryService
{
    public async Task<Result<CursorResponse<FoodRecordSearchResponse>?>> GetFoodRecordSearchAsync(
        GetFoodRecordSearchQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<CursorResponse<FoodRecordSearchResponse>?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var query = context.FoodRecords
            .AsNoTracking()
            .ApplySearchFilters(request)
            .Where(f =>
                f.RestaurantId == request.RestaurantId &&
                !f.IsDeleted)
            .OrderByDescending(f => f.CreatedAt)
            .ProjectToType<FoodRecordSearchResponse>();

        var result = await CursorResponse<FoodRecordSearchResponse>.ToCursorResultAsync(
            query,
            request.PageSize,
            f => f.CreatedAt.ToString("O"), // Use ISO 8601 format for cursor
            cancellationToken);

        return Result<CursorResponse<FoodRecordSearchResponse>?>.SuccessResult(result);
    }

    public async Task<Result<FoodRecordDetailResponse?>> GetFoodRecordByIdAsync(
        GetFoodRecordByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        // Check cache first
        var cacheKey = $"FoodRecord_{request.Id}";
        var cachedFoodRecord = await cacheService.GetAsync<FoodRecordDetailResponse>(cacheKey, cancellationToken);
        if (cachedFoodRecord != null)
        {
            return Result<FoodRecordDetailResponse?>.SuccessResult(cachedFoodRecord);
        }

        // Find food record
        var foodRecord = await context.FoodRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(f =>
                f.Id == request.Id &&
                !f.IsDeleted,
                cancellationToken);

        if (foodRecord == null)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.NotFound("Food record not found."));
        }

        var response = foodRecord.Adapt<FoodRecordDetailResponse>();

        // Cache the result
        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30), cancellationToken);

        return Result<FoodRecordDetailResponse?>.SuccessResult(response);
    }
}

// use for filter
public static class FoodRecordSearchExtention
{
    public static IQueryable<FoodRecord> ApplySearchFilters(
        this IQueryable<FoodRecord> query,
        GetFoodRecordSearchQuery request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            query = query.Where(f => !string.IsNullOrWhiteSpace(f.FoodName) && f.FoodName.Contains(request.SearchString));
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(f => f.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(f => f.Price <= request.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.RestaurantId.ToString()))
        {
            query = query.Where(f => f.RestaurantId == request.RestaurantId);
        }

        return query;
    }
}
