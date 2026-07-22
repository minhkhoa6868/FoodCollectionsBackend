using FoodCollectionsBackend.Domain.Common;
namespace FoodCollectionsBackend.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    // Many-to-One relationship with User
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
