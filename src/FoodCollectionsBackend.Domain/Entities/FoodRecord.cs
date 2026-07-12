using FoodCollectionsBackend.Domain.Common;
namespace FoodCollectionsBackend.Domain.Entities;

public class FoodRecord : SoftDeletableEntity
{
    public Guid Id { get; set; }
    public string? FoodName { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string? RestaurantName { get; set; }
    public string? Address { get; set; }
    public string? PlaceId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Rating { get; set; }
    public bool IsVisited { get; set; }
    public DateTime? VisitedDate { get; set; }
    public string? Notes { get; set; }

    // Many-to-One relationship with Category
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    // One-to-Many relationship with FoodImage
    public ICollection<FoodImage>? FoodImages { get; set; }
}
