using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.FoodCatalogContext.Domain.Services;

public interface IComidaRepository: IBaseRepository<Comida>
{
    //Listar comidas por el nombre
    Task<Comida?> FindByNameAsync(string name);

    //Listar comidas por un tipo específico
    
    Task<Comida?> FindComidaByIdAsync(int id);

    Task<IEnumerable<Comida>> ListByTipoComidaAsync(int idTipoComida);

    //Task<IEnumerable<Comida>> ListSpecialComidasAsync();

    //Task<IEnumerable<Comida>> ListByTipoComidaIdAsync(int tipoComidaId);


}



