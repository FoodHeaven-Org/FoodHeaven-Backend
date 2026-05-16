namespace food_heaven_backend.FoodCatalogContext.Interfaces.REST.Resources;

public record ComidaResource(
    int id_comida,
    string nombre,
    string complemento,
    string url,
    NutrienteResource nutriente,
    int id_proveedor,
    int id_tipo_comida,
    int es_especial
);

public record NutrienteResource(
    int cal,
    int prote,
    int carbo,
    int grasa
);