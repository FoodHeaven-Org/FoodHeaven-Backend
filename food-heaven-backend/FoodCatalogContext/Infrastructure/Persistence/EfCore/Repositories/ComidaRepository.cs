using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Repositories;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.FoodCatalogContext.Infrastructure.Persistence.EfCore.Repositories;

public class ComidaRepository(FoodHeavenContext context)
    : BaseRepository<Comida>(context), IComidaRepository
{
    public async Task<Comida?> FindByNameAsync(string nombre)
    {
        return await Context.Set<Comida>()
            .FirstOrDefaultAsync(p => p.Nombre == nombre);
    }

    public async Task<IEnumerable<Comida>> ListByTipoComidaAsync(int idTipoComida)
    {
        return await Context.Set<Comida>()
            .Where(c => c.TipoComida.Id == idTipoComida)
            .Include(c => c.TipoComida)
            .ToListAsync();
    }

    public async Task<Comida?> FindComidaByIdAsync(int id)
    {
        return await Context.Set<Comida>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> AllMealIdsExistAsync(IEnumerable<int> mealIds)
    {
        var distinctMealIds = mealIds
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (distinctMealIds.Length == 0) return false;

        var existingMealIdsCount = await Context.Set<Comida>()
            .Where(c => distinctMealIds.Contains(c.Id))
            .Select(c => c.Id)
            .Distinct()
            .CountAsync();

        return existingMealIdsCount == distinctMealIds.Length;
    }
}
