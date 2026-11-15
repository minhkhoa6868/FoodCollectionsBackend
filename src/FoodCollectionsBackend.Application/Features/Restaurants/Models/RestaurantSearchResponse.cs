namespace FoodCollectionsBackend.Application.Features.Restaurants.Models;

public class RestaurantSearchResponse
{
    public Guid Id { get; set; }
    public string? RestaurantName { get; set; }
    public bool? IsVisited { get; set; } = false;
    public int NumberOfFoodRecords { get; set; } = 0;
}
