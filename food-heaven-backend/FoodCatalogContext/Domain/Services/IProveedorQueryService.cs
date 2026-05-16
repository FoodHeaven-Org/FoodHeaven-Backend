using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Queries;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IProveedorQueryService
{
    Task<IEnumerable<Proveedor>> Handle(GetAllProvidersQuery query);
    Task<Proveedor?> Handle(GetProviderByIdQuery query);
}
