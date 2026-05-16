namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;

public record DeleteComidaCommand
{
    public int Id { get; init; }

    public DeleteComidaCommand(int id)
    {
        if (id <= 0) throw new ArgumentException("Id must be greater than zero.", nameof(id));
        Id = id;
    }
}
