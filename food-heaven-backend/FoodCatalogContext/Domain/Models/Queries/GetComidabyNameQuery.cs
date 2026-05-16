namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;

public record GetComidabyNameQuery
{
    public GetComidabyNameQuery(string name)
    {
        Name = name;
    }
    public string Name { get; init; }

}