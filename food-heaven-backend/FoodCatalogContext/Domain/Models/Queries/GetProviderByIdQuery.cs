namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Queries
{

    public record GetProviderByIdQuery
    {
        public GetProviderByIdQuery(int providerId)
        {
            ProviderId = providerId;
        }

        public int ProviderId { get; init; }
    }
}