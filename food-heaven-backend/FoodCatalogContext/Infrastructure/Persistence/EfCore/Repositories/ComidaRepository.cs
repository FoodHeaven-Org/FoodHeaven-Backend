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
    //public async Task<IEnumerable<Comida>> ListSpecialComidasAsync()
    //{
    //    return await Context.Set<Comida>()
    //        .Where(c => c.EsEspecial)
    //        .ToListAsync();
    //}
    //
    //public async Task<IEnumerable<Comida>> ListByTipoComidaIdAsync(int tipoComidaId)
    //{
    //    return await Context.Set<Comida>()
    //        .Where(c => c.Id_TipoComida == tipoComidaId)
    //        .Include(c => c.TipoComida)  // Incluye el TipoComida relacionado
    //        .ToListAsync();
    //}


}
