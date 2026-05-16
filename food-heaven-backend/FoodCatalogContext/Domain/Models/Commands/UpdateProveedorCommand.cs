namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Commands
{
    public class UpdateProveedorCommand
    {
        public int Id { get; init; }
        public string Nombre { get; init; }
        public string Distrito { get; init; }
        public string Contacto { get; init; }
        public int TipoProveedorId { get; init; }

        public UpdateProveedorCommand(int id, string nombre, string distrito, string contacto, int tipoProveedorId)
        {
            Id = id;
            Nombre = nombre;
            Distrito = distrito;
            Contacto = contacto;
            TipoProveedorId = tipoProveedorId;
        }
    }
}