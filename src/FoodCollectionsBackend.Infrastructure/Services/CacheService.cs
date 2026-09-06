using FoodCollectionsBackend.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace FoodCollectionsBackend.Infrastructure.Services;

public class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var data = await cache.GetStringAsync(key, cancellationToken);
        return data is null ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        => await cache.RemoveAsync(key, cancellationToken);
}
