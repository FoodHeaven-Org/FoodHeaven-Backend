using food_heaven_backend.Shared.Domain;
using food_heaven_backend.Shared.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;

namespace food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Repositories;

public class UnitOfWork(FoodHeavenContext context) : IUnitOfWork
{
    /// <inheritdoc />
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}