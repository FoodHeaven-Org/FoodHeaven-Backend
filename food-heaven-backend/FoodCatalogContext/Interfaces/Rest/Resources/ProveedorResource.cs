using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.Rest.Resources;

public record ProveedorResource(string nombre, string distrito, string contacto, int tipoProveedorId)
{
    public static ProveedorResource FromEntity(Proveedor proveedor)
    {
        return new ProveedorResource(
            proveedor.Nombre,
            proveedor.Distrito,
            proveedor.Contacto,
            proveedor.TipoProveedorId
        );
    }
}
