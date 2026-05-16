using food_heaven_backend.FoodCatalogContext.Domain.Repositories;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration;
using food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.FoodCatalogContext.Infrastructure.Persistence.EfCore.Repositories;

public class ProveedorRepository(FoodHeavenContext context)
    : BaseRepository<Proveedor>(context), IProveedorRepository
{
    public async Task<Proveedor?> FindByNombreAsync(string nombre)
    {
        return await Context.Set<Proveedor>()
            .FirstOrDefaultAsync(p => p.Nombre == nombre);
    }

    public async Task<IEnumerable<Proveedor>> GetAllWithTipoProveedorAsync()
    {
        return await Context.Set<Proveedor>()
            .Include(p => p.TipoProveedor)
            .ToListAsync();
    }
    
    
}