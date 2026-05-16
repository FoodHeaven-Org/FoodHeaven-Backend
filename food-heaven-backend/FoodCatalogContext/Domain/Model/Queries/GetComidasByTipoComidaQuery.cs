namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Queries;

public record GetComidasByTipoComidaQuery
{
    public int id_tipo_comida { get; init; }

    public GetComidasByTipoComidaQuery(int id_tipo_comida)
    {
        if (id_tipo_comida <= 0) throw new ArgumentException("id_tipo_comida must be greater than zero.", nameof(id_tipo_comida));
        this.id_tipo_comida = id_tipo_comida;
    }
}
