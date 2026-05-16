using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.PlanComidas.Domain.Models.Entities;

[Table("PlanComida")]
public class PlanComida : BaseEntity
{

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime FechaFin { get; set; }
    
    [Column("lista_comidas")]
    public int[] ListaComidas { get; set; }

    public PlanComida(int idUsuario, DateTime fechaInicio, DateTime fechaFin, int[] listaComidas)
    {
        IdUsuario = idUsuario;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        ListaComidas = listaComidas;
    }

    public PlanComida() { }
}
