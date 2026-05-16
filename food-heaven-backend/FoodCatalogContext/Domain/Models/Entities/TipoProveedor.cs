using System;
using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.FoodCatalogContext.Domain.Models.valueobjects;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;

[Table("TipoProveedor")]
public class TipoProveedor : BaseEntity
{
    [Column("id_tipo_proveedor")]
    public new int Id { get; set; }

    [Column("descripcion", TypeName = "varchar(50)")]
    public TipoProveedorDescripcion Descripcion { get; set; }

    public TipoProveedor(TipoProveedorDescripcion descripcion)
    {
        Descripcion = descripcion;
    }

    public TipoProveedor() { }
}