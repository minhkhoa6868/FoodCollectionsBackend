using FoodCollectionsBackend.Domain.Common;
namespace FoodCollectionsBackend.Domain.Entities;

public class FoodImage : BaseEntity
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }

    // Many-to-One relationship with FoodRecord
    public Guid FoodRecordId { get; set; }
    public FoodRecord? FoodRecord { get; set; }
}
