namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Commands
{
    public class DeleteProveedorCommand
    {
        public int Id { get; init; }

        public DeleteProveedorCommand(int id)
        {
            Id = id;
        }
    }
}