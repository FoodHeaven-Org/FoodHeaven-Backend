using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.FoodCatalogContext.Interfaces.REST.Resources;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.REST.Transform;

public static class ComidaResourceFromEntityAssembler
{
    public static ComidaResource ToResourceFromEntity(Comida comida)
    {
        return new ComidaResource
        (
            id_comida: comida.Id,
            nombre: comida.Nombre,
            complemento: comida.Complemento,
            url: comida.Url,
            nutriente: new NutrienteResource(
                cal: comida.Cal,
                prote: comida.Prote,
                carbo: comida.Carbo,
                grasa: comida.Grasa
            ),
            id_proveedor: comida.Id_Proveedor,
            id_tipo_comida: comida.id_tipo_comida,
            es_especial: comida.es_especial // ya es int
        );
    }
}