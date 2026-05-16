using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IProveedorQueryService
{
    Task<IEnumerable<Proveedor>> Handle(GetAllProvidersQuery query);
    Task<Proveedor> Handle(GetProviderByIdQuery query);
}