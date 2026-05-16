namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;

public class UpdateComidaCommand
{
    public string Nombre { get; set; }
    
    public string Complemento { get; set; }
    public string Url { get; set; }
    public int Calorias { get; set; }
    public int Proteina { get; set; }
    public int Carbohidrato { get; set; }
    public int Grasa { get; set; }
    public int id_tipo_comida { get; set; }
    public int Id_proveedor { get; set; }
    public bool es_especial { get; set; }

    public UpdateComidaCommand() { }

    public UpdateComidaCommand(string nombre, string complemento, string url, int calorias, int proteina, int carbohidrato, int grasa, int idTipoComida, int idProveedor, bool es_especial)
    {
        Nombre = nombre;
        Complemento = complemento;
        Url = url;
        Calorias = calorias;
        Proteina = proteina;
        Carbohidrato = carbohidrato;
        Grasa = grasa;
        id_tipo_comida = idTipoComida;
        Id_proveedor = idProveedor;
        es_especial = es_especial;
    }
}