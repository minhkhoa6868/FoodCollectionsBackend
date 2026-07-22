namespace FoodCollectionsBackend.Application.Interfaces;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Username { get; }
    string? Email { get; }
}
