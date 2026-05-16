using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IComidaCommandService
{
    Task<Comida> Handle(CreateComidaCommand command);
    Task<bool> Handle(UpdateComidaCommand command, int id);
    Task<bool> Handle(DeleteComidaCommand command);
}