using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;

[Table("Proveedor")]
public class Proveedor : BaseEntity
{
    [Column("id_proveedor")]
    public new int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("distrito")]
    public string Distrito { get; set; } = string.Empty;

    [Column("contacto")]
    public string Contacto { get; set; } = string.Empty;

    [Column("id_tipo_proveedor")]
    public int TipoProveedorId { get; set; }

    [ForeignKey(nameof(TipoProveedorId))] 
    public TipoProveedor TipoProveedor { get; set; } = null!;

    public Proveedor(string nombre, string distrito, string contacto, int tipoProveedorId)
    {
        Nombre = nombre;
        Distrito = distrito;
        Contacto = contacto;
        TipoProveedorId = tipoProveedorId;
    }

    public Proveedor() { }
}