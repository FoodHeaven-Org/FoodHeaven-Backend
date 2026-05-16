using food_heaven_backend.FoodCatalogContext.Domain;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.Shared.Infraestructure.Persistence.Configuration;
using food_heaven_backend.Shared.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace food_heaven_backend.FoodCatalogContext.Infraestructure;

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