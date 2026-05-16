namespace food_heaven_backend.PlanComidas.Domain.Model.Commands;

public record DeletePlanComidaCommand
{
    public int Id { get; init; }

    public DeletePlanComidaCommand(int id)
    {
        if (id <= 0) throw new ArgumentException("Id must be greater than zero.", nameof(id));
        Id = id;
    }
}
