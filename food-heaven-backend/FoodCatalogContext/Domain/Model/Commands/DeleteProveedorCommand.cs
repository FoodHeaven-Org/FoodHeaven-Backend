namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Commands
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