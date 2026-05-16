namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Commands
{
    public class DeleteComidaCommand
    {
        public int Id { get; init; }

        public DeleteComidaCommand(int id)
        {
            Id = id;
        }
    }
}