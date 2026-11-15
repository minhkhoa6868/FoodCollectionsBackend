namespace FoodCollectionsBackend.Application.Features.FoodRecords.Models;

public class FoodRecordSearchResponse
{
    public Guid Id { get; set; }
    public string? FoodName { get; set; }
    public decimal? Price { get; set; }
    public Guid? RestaurantId { get; set; }
    public decimal? Rating { get; set; }
    public bool IsTried { get; set; }
    public DateTime CreatedAt { get; set; }
}
