using FoodCollectionsBackend.Domain.Common;
namespace FoodCollectionsBackend.Domain.Entities;

public class FoodRecord : SoftDeletableEntity
{
    public Guid Id { get; set; }
    public string? FoodName { get; set; }
    public decimal? Price { get; set; }
    public decimal? Rating { get; set; }
    public bool? IsTried { get; set; } = false;
    public string? Notes { get; set; }

    // Many-to-One relationship with Restaurant
    public Guid RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    // One-to-Many relationship with FoodImage
    public ICollection<FoodImage> FoodImages { get; set; } = [];
}
