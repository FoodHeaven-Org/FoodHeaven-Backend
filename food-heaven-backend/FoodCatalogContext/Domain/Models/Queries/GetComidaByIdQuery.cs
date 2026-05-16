namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;

public class GetComidaByIdQuery
{
    public GetComidaByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; init; }
}