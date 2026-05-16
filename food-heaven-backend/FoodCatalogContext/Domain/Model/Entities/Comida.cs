using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;

[Table("Comida")]
public class Comida : BaseEntity
{
    [Column("id_comida")]
    public new int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("nombre_en")]
    public string NombreEn { get; set; } = string.Empty;

    [Column("complemento")]
    public string Complemento { get; set; } = string.Empty;

    [Column("complemento_en")]
    public string ComplementoEn { get; set; } = string.Empty;

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
    public int CatalogSourceId { get; set; } = 1;

    [Column("id_tipo_comida")]
    public int id_tipo_comida { get; set; }

    [ForeignKey(nameof(id_tipo_comida))]
    public TipoComida TipoComida { get; set; } = null!;

    [Column("es_especial")]
    public int es_especial { get; set; } // se usa como int en el mock API

    public Comida(
        string nombre,
        string complemento,
        string url,
        int cal,
        int prote,
        int carbo,
        int grasa,
        int idTipoComida,
        int es_especial,
        string nombreEn = "",
        string complementoEn = "")
    {
        Nombre = nombre;
        NombreEn = nombreEn;
        Complemento = complemento;
        ComplementoEn = complementoEn;
        Url = url;
        Cal = cal;
        Prote = prote;
        Carbo = carbo;
        Grasa = grasa;
        id_tipo_comida = idTipoComida;
        this.es_especial = es_especial;
    }

    public Comida() { }
}
