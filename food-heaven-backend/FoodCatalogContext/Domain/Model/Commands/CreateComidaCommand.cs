public class CreateComidaCommand
{
    public string Nombre { get; init; }
    public string Complemento { get; init; }
    public string Url { get; init; }
    public int Calorias { get; init; }
    public int Proteina { get; init; }
    public int Carbohidrato { get; init; }
    public int Grasa { get; init; }
    public int id_tipo_comida { get; init; }
    public int Id_proveedor { get; init; }
    public bool es_especial { get; init; }

    public CreateComidaCommand(
        string nombre,
        string complemento,
        string url,
        int calorias,
        int proteina,
        int carbohidrato,
        int grasa,
        int id_tipo_comida,
        int id_proveedor,
        bool es_especial)
    {
        Nombre = nombre;
        Complemento = complemento;
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