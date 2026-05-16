namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Exceptions
{
    public class ProveedorNotFoundException : Exception
    {
        public ProveedorNotFoundException()
            : base("Proveedor not found.")
        {
        }

        public ProveedorNotFoundException(string message)
            : base(message)
        {
        }

        public ProveedorNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ProveedorNotFoundException(int id)
            : base($"Proveedor with ID {id} was not found.")
        {
        }
    }
}