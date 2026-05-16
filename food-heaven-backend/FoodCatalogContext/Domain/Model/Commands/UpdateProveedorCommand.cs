namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;

public record UpdateProveedorCommand
{
    public string Nombre { get; init; }
    public string Distrito { get; init; }
    public string Contacto { get; init; }
    public int TipoProveedorId { get; init; }

    public UpdateProveedorCommand(string nombre, string distrito, string contacto, int tipoProveedorId)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre is required.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(distrito)) throw new ArgumentException("Distrito is required.", nameof(distrito));
        if (string.IsNullOrWhiteSpace(contacto)) throw new ArgumentException("Contacto is required.", nameof(contacto));
        if (tipoProveedorId <= 0) throw new ArgumentException("TipoProveedorId must be greater than zero.", nameof(tipoProveedorId));

        Nombre = nombre;
        Distrito = distrito;
        Contacto = contacto;
        TipoProveedorId = tipoProveedorId;
    }
}
