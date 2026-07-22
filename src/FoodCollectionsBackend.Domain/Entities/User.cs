using FoodCollectionsBackend.Domain.Common;
namespace FoodCollectionsBackend.Domain.Entities;

public class User : SoftDeletableEntity
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    // One-to-Many relationship with Category
    public ICollection<Category>? Categories { get; set; }

    // One-to-Many relationship with RefreshToken
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}
