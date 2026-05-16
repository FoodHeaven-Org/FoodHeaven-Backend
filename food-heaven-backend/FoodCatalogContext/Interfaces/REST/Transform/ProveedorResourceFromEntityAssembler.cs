using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.FoodCatalogContext.Interfaces.REST.Resources;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.REST.Transform;

public class ProveedorResourceFromEntityAssembler
{
    public static ProveedorResource ToResourceFromEntity(Proveedor proveedor)
    {
        return new ProveedorResource(
            
            proveedor.Nombre,
            proveedor.Distrito,
            proveedor.Contacto,
            proveedor.TipoProveedorId
        );
    }
    
}