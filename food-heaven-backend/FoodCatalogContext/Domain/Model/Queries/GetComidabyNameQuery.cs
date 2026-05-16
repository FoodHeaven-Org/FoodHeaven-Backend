namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Queries;

public record GetComidabyNameQuery
{
    public string Name { get; init; }

    public GetComidabyNameQuery(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        Name = name;
    }
}
