using food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IProveedorCommandService
{
    Task<Proveedor> Handle(CreateProveedorCommand command);
    Task<bool> Handle(UpdateProveedorCommand command, int id);
    Task<bool> Handle(DeleteProveedorCommand command);
}