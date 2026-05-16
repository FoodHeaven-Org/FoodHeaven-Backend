namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Exceptions;

public class ComidaNotFoundException : Exception
{
    public ComidaNotFoundException()
        : base("Comida not found.")
    {
    }

    public ComidaNotFoundException(string message)
        : base(message)
    {
    }

    public ComidaNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public ComidaNotFoundException(int id)
        : base($"Comida with ID {id} was not found.")
    {
    }
}