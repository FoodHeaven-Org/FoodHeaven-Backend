using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;

[Table("Comida")]
public class Comida : BaseEntity
{
    [Column("id_comida")]
    public new int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("complemento")]
    public string Complemento { get; set; } = string.Empty;

    [Column("url")]
    public string Url { get; set; } = string.Empty;

    [Column("cal")]
    public int Cal { get; set; }

    [Column("prote")]
    public int Prote { get; set; }

    [Column("carbo")]
    public int Carbo { get; set; }

    [Column("grasa")]
    public int Grasa { get; set; }

    [Column("id_proveedor")]
    public int Id_Proveedor { get; set; }

    [ForeignKey(nameof(Id_Proveedor))]
    public Proveedor Proveedor { get; set; } = null!;

    [Column("id_tipo_comida")]
    public int id_tipo_comida { get; set; }

    [ForeignKey(nameof(id_tipo_comida))]
    public TipoComida TipoComida { get; set; } = null!;

    [Column("es_especial")]
    public int es_especial { get; set; } // se usa como int en el mock API

    public Comida(string nombre, string complemento, string url, int cal, int prote, int carbo, int grasa, int idProveedor, int idTipoComida, int es_especial)
    {
        Nombre = nombre;
        Complemento = complemento;
        Url = url;
        Cal = cal;
        Prote = prote;
        Carbo = carbo;
        Grasa = grasa;
        Id_Proveedor = idProveedor;
        id_tipo_comida = idTipoComida;
        es_especial = es_especial;
    }

    public Comida() { }
}