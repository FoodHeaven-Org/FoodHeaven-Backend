namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;

public record UpdateComidaCommand
{
    public string Nombre { get; init; }
    public string NombreEn { get; init; }
    public string Complemento { get; init; }
    public string ComplementoEn { get; init; }
    public string Url { get; init; }
    public int Calorias { get; init; }
    public int Proteina { get; init; }
    public int Carbohidrato { get; init; }
    public int Grasa { get; init; }
    public int id_tipo_comida { get; init; }
    public int Id_proveedor { get; init; }
    public bool es_especial { get; init; }

    public UpdateComidaCommand(
        string nombre,
        string? nombreEn,
        string complemento,
        string? complementoEn,
        string url,
        int calorias,
        int proteina,
        int carbohidrato,
        int grasa,
        int id_tipo_comida,
        int id_proveedor,
        bool es_especial)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre is required.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(complemento)) throw new ArgumentException("Complemento is required.", nameof(complemento));
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Url is required.", nameof(url));
        if (id_tipo_comida <= 0) throw new ArgumentException("id_tipo_comida must be greater than zero.", nameof(id_tipo_comida));
        if (id_proveedor <= 0) throw new ArgumentException("id_proveedor must be greater than zero.", nameof(id_proveedor));

        Nombre = nombre;
        NombreEn = nombreEn?.Trim() ?? string.Empty;
        Complemento = complemento;
        ComplementoEn = complementoEn?.Trim() ?? string.Empty;
        Url = url;
        Calorias = calorias;
        Proteina = proteina;
        Carbohidrato = carbohidrato;
        Grasa = grasa;
        this.id_tipo_comida = id_tipo_comida;
        Id_proveedor = id_proveedor;
        this.es_especial = es_especial;
    }
}
