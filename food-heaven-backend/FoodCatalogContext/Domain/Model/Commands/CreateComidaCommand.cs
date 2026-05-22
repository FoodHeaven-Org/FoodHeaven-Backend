namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;

public record CreateComidaCommand
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

    public CreateComidaCommand(
        string nombre,
        string? nombreEn,
        string complemento,
        string? complementoEn,
        string url,
        int calorias,
        int proteina,
        int carbohidrato,
        int grasa,
        int id_tipo_comida)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre is required.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(complemento)) throw new ArgumentException("Complemento is required.", nameof(complemento));
        EnsureMealDetailsAreValid(url, calorias, proteina, carbohidrato, grasa, id_tipo_comida);

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
    }

    private static void EnsureMealDetailsAreValid(
        string url,
        int calorias,
        int proteina,
        int carbohidrato,
        int grasa,
        int idTipoComida)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Url is required.", nameof(url));
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Url must be an absolute HTTP or HTTPS URL.", nameof(url));
        }

        if (idTipoComida is < 1 or > 3)
            throw new ArgumentException("id_tipo_comida must be 1 (breakfast), 2 (lunch), or 3 (dinner).", nameof(idTipoComida));

        if (calorias <= 0) throw new ArgumentException("Calorias must be greater than zero.", nameof(calorias));
        if (proteina < 0) throw new ArgumentException("Proteina cannot be negative.", nameof(proteina));
        if (carbohidrato < 0) throw new ArgumentException("Carbohidrato cannot be negative.", nameof(carbohidrato));
        if (grasa < 0) throw new ArgumentException("Grasa cannot be negative.", nameof(grasa));
    }
}
