using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.FoodCatalogContext.Domain.Repositories;

public interface IComidaRepository: IBaseRepository<Comida>
{
    Task<Comida?> FindByNameAsync(string name);

    Task<Comida?> FindComidaByIdAsync(int id);

    Task<IEnumerable<Comida>> ListByTipoComidaAsync(int idTipoComida);

    Task<bool> AllMealIdsExistAsync(IEnumerable<int> mealIds);
}
