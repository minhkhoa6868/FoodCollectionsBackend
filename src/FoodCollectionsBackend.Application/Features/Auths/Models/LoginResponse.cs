namespace FoodCollectionsBackend.Application.Features.Auths.Models;

public class LoginResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expires { get; set; }
    public string? Username { get; set; }
}
