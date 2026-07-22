using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Domain.Entities;

namespace FoodCollectionsBackend.Application.Interfaces.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Category> Categories { get; }
    DbSet<FoodRecord> FoodRecords { get; }
    DbSet<FoodImage> FoodImages { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
