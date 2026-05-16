using food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IComidaCommandService
{
    Task<Comida> Handle(CreateComidaCommand command);
    Task<bool> Handle(UpdateComidaCommand command, int id);
    Task<bool> Handle(DeleteComidaCommand command);
}