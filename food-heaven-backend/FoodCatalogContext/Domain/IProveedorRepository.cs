using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.FoodCatalogContext.Domain;

public interface IProveedorRepository : IBaseRepository<Proveedor>
{
    Task<Proveedor?> FindByNombreAsync(string nombre);
    Task<IEnumerable<Proveedor>> GetAllWithTipoProveedorAsync();
}