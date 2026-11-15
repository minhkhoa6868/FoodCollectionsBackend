namespace FoodCollectionsBackend.Application.Features.FoodRecords.Models;

public class FoodRecordDetailResponse
{
    public Guid Id { get; set; }
    public string? FoodName { get; set; }
    public decimal? Price { get; set; }
    public decimal? Rating { get; set; }
    public bool? IsTried { get; set; } = false;
    public string? Notes { get; set; }
}


