namespace FoodCollectionsBackend.Application.Features.Categories.Models;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public string? ColorHex { get; set; }
}
