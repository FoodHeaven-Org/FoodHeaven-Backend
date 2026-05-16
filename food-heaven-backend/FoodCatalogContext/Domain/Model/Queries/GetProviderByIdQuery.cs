namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Queries
{

    public record GetProviderByIdQuery
    {
        public int ProviderId { get; init; }

        public GetProviderByIdQuery(int providerId)
        {
            if (providerId <= 0) throw new ArgumentException("ProviderId must be greater than zero.", nameof(providerId));
            ProviderId = providerId;
        }
    }
}
