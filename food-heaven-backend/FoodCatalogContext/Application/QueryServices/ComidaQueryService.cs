using food_heaven_backend.FoodCatalogContext.Domain;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;
using food_heaven_backend.FoodCatalogContext.Domain.Services;

namespace food_heaven_backend.FoodCatalogContext.Application.QueryServices
{
    public class ComidaQueryService(IComidaRepository comidaRepository) : IComidaQueryService
    {
        private readonly IComidaRepository _comidaRepository = comidaRepository ?? throw new ArgumentNullException(nameof(comidaRepository));

        public async Task<IEnumerable<Comida>> Handle(GetAllComidaQuery query)
        {
            var comidas = await _comidaRepository.ListAsync();
            return comidas ?? Enumerable.Empty<Comida>();
        }


        public async Task<Comida?> Handle(GetComidaByIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var comida = await _comidaRepository.FindComidaByIdAsync(query.Id);
            return comida;
        }

        public async Task<IEnumerable<Comida>> Handle(GetComidaByIdTipoComida query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var comida = await _comidaRepository.ListByTipoComidaAsync(query.id_tipo_comida);
            return comida;
        }

    }
}