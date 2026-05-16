namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Commands
{
    public class CreateProveedorCommand
    {
        public string Nombre { get; init; }
        public string Distrito { get; init; }
        public string Contacto { get; init; }
        public int TipoProveedorId { get; init; }

        public CreateProveedorCommand(string nombre, string distrito, string contacto, int tipoProveedorId)
        {
            Nombre = nombre;
            Distrito = distrito;
            Contacto = contacto;
            TipoProveedorId = tipoProveedorId;
        }
    }
}