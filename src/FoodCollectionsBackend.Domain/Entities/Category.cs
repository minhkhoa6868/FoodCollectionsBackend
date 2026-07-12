using FoodCollectionsBackend.Domain.Common;
namespace FoodCollectionsBackend.Domain.Entities;

public class Category : SoftDeletableEntity
{
    public Guid Id { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public string? ColorHex { get; set; }

    // Many-to-One relationship with User
    public Guid UserId { get; set; }
    public User? User { get; set; }

    // One-to-Many relationship with FoodRecord
    public ICollection<FoodRecord>? FoodRecords { get; set; }
}
