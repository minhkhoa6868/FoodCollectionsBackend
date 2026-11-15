using FoodCollectionsBackend.Application.Interfaces.FoodRecords;
using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Application.Features.FoodRecords.Commands;
using FoodCollectionsBackend.Domain.Entities;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Common.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FoodCollectionsBackend.Infrastructure.Services.FoodRecords;

public class FoodRecordCommandService(
    IAppDbContext context,
    ICurrentUser currentUser,
    ICacheService cacheService) : IFoodRecordCommandService
{
    public async Task<Result<FoodRecordDetailResponse?>> CreateFoodRecordAsync(
        CreateFoodRecordCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var foodRecord = new FoodRecord
        {
            FoodName = request.FoodName?.Trim(),
            Price = request.Price,
            Rating = request.Rating,
            IsTried = request.IsTried,
            Notes = request.Notes?.Trim(),
            RestaurantId = request.RestaurantId
        };

        try
        {
            await context.FoodRecords.AddAsync(foodRecord, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<FoodRecordDetailResponse?>.SuccessResult(foodRecord.Adapt<FoodRecordDetailResponse>());
        }
        catch (Exception ex)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result<FoodRecordDetailResponse?>> UpdateFoodRecordAsync(
        UpdateFoodRecordCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var foodRecord = await context.FoodRecords
            .FirstOrDefaultAsync(f =>
                f.Id == request.Id &&
                !f.IsDeleted,
                cancellationToken);

        if (foodRecord == null)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.NotFound("Food record not found."));
        }

        if (!string.IsNullOrWhiteSpace(request.FoodName) && request.FoodName.Trim() != foodRecord.FoodName)
        {
            foodRecord.FoodName = request.FoodName.Trim();
        }

        if (request.Price.HasValue && request.Price != foodRecord.Price)
        {
            foodRecord.Price = request.Price;
        }

        if (request.Rating.HasValue && request.Rating != foodRecord.Rating)
        {
            foodRecord.Rating = request.Rating;
        }

        if (request.IsTried != foodRecord.IsTried)
        {
            foodRecord.IsTried = request.IsTried;
        }

        if (!string.IsNullOrWhiteSpace(request.Notes) && request.Notes.Trim() != foodRecord.Notes)
        {
            foodRecord.Notes = request.Notes.Trim();
        }

        if (request.RestaurantId != foodRecord.RestaurantId)
        {
            foodRecord.RestaurantId = request.RestaurantId;
        }

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveAsync($"FoodRecord_{foodRecord.Id}", cancellationToken);
            return Result<FoodRecordDetailResponse?>.SuccessResult(foodRecord.Adapt<FoodRecordDetailResponse>());
        }
        catch (Exception ex)
        {
            return Result<FoodRecordDetailResponse?>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result> DeleteFoodRecordAsync(
        DeleteFoodRecordCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var foodRecords = await context.FoodRecords
            .Where(f => request.Ids.Contains(f.Id) && !f.IsDeleted)
            .ToListAsync(cancellationToken);

        if (foodRecords.Count == 0)
        {
            return Result.FailResult(Error.NotFound("No food records found to delete."));
        }

        try
        {
            context.FoodRecords.RemoveRange(foodRecords);
            await context.SaveChangesAsync(cancellationToken);
            foreach (var foodRecord in foodRecords)
            {
                await cacheService.RemoveAsync($"FoodRecord_{foodRecord.Id}", cancellationToken);
            }
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            return Result.FailResult(Error.Invalid(ex.Message));
        }
    }
}
