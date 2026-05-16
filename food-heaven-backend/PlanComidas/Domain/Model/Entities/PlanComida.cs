using System.ComponentModel.DataAnnotations.Schema;
using food_heaven_backend.Shared.Domain.Model.Entities;

namespace food_heaven_backend.PlanComidas.Domain.Model.Entities;

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
    public int[] ListaComidas { get; set; } = Array.Empty<int>();

    public PlanComida(int idUsuario, DateTime fechaInicio, DateTime fechaFin, int[] listaComidas)
    {
        if (idUsuario <= 0) throw new ArgumentException("IdUsuario must be greater than zero.", nameof(idUsuario));
        if (fechaFin <= fechaInicio) throw new ArgumentException("FechaFin must be after FechaInicio.", nameof(fechaFin));
        if (listaComidas is not { Length: 21 }) throw new ArgumentException("ListaComidas must contain exactly 21 meal ids.", nameof(listaComidas));

        IdUsuario = idUsuario;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        ListaComidas = listaComidas;
    }

    public PlanComida() { }
}
