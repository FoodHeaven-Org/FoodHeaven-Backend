using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IComidaQueryService
{
    Task<IEnumerable<Comida>> Handle(GetAllComidaQuery query);
    Task<Comida> Handle(GetComidaByIdQuery query);
    Task<IEnumerable<Comida>> Handle(GetComidaByIdTipoComida query);

}
