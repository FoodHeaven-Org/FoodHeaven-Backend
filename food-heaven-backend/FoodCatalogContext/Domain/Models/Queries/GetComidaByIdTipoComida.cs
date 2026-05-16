namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;

public class GetComidaByIdTipoComida
{
    public GetComidaByIdTipoComida(int Id_tipo_comida)
    {
        id_tipo_comida = Id_tipo_comida;
    }

    public int id_tipo_comida { get; init; }
}