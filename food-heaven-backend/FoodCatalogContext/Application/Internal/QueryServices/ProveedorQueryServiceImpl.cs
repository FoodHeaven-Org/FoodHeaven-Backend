using food_heaven_backend.FoodCatalogContext.Domain.Repositories;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Queries;
using food_heaven_backend.FoodCatalogContext.Domain.Services;


namespace food_heaven_backend.FoodCatalogContext.Application.Internal.QueryServices
{
    public class ProveedorQueryServiceImpl(IProveedorRepository proveedorRepository) : IProveedorQueryService
    {
        private readonly IProveedorRepository _proveedorRepository = proveedorRepository ?? throw new ArgumentNullException(nameof(proveedorRepository));

        public async Task<IEnumerable<Proveedor>> Handle(GetAllProvidersQuery query)
        {
            var proveedores = await _proveedorRepository.ListAsync();
            return proveedores ?? Enumerable.Empty<Proveedor>();
        }

        public async Task<Proveedor?> Handle(GetProviderByIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var proveedor = await _proveedorRepository.FindByIdAsync(query.ProviderId);
            return proveedor;
        }
    }
}   