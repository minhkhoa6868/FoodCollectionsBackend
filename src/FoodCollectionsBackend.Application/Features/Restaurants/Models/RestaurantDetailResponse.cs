namespace FoodCollectionsBackend.Application.Features.Restaurants.Models;

public class RestaurantDetailResponse
{
    public Guid Id { get; set; }
    public string? RestaurantName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Address { get; set; }
    public string? PlaceId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsVisited { get; set; } = false;
    public DateTime? VisitedDate { get; set; }
    public string? Notes { get; set; }
}
