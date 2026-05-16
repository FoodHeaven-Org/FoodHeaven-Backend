using food_heaven_backend.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.FoodCatalogContext.Domain.Models.ValueObjects;

namespace food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;

[Table("TipoComida")]
public class TipoComida: BaseEntity
{
    
    [Column("id_tipo_Comida")]
    public new int Id { get; set; }
    
    [Column("descripcion", TypeName = "varchar(50)")]
    public TipoComidas Descripcion { get; set; }
    
    //[Column("id_comida")]
    //public int IdComida { get; set; }
    
    //public Comida Comida { get; set; }

    
    
    public TipoComida(TipoComidas descripcion, int idComida)
    {
        //IdComida = idComida;
        Descripcion = descripcion;
    }

    public TipoComida()
    {
        
    }

}