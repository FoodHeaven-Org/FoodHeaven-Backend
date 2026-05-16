using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.Rest.Resources;

public record ComidaResource(
    int id_comida,
    string nombre,
    string complemento,
    string url,
    NutrienteResource nutriente,
    int id_proveedor,
    int id_tipo_comida,
    int es_especial
)
{
    public static ComidaResource FromEntity(Comida comida)
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
            es_especial: comida.es_especial
        );
    }
}

public record NutrienteResource(
    int cal,
    int prote,
    int carbo,
    int grasa
);
