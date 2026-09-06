using Microsoft.Extensions.Caching.Memory;

namespace FoodCollectionsBackend.Infrastructure.Services;

public class OtpService(IMemoryCache cache)
{
    public static string GenerateOtp()
        => Random.Shared.Next(100000, 999999).ToString();

    public void StoreOtp(string email, string otp)
        => cache.Set($"otp:{email}", otp, TimeSpan.FromMinutes(5));

    public bool VerifyOtp(string email, string otp)
    {
        if (cache.TryGetValue($"otp:{email}", out string? stored) && stored == otp)
        {
            cache.Remove($"otp:{email}");
            return true;
        }
        return false;
    }
}
