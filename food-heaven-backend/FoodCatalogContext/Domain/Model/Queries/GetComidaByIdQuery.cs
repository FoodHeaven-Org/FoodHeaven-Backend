namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Queries;

public record GetComidaByIdQuery
{
    public int Id { get; init; }

    public GetComidaByIdQuery(int id)
    {
        if (id <= 0) throw new ArgumentException("Id must be greater than zero.", nameof(id));
        Id = id;
    }
}
