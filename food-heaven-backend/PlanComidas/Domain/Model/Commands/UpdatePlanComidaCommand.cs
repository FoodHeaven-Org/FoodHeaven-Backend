namespace food_heaven_backend.PlanComidas.Domain.Model.Commands;

public record UpdatePlanComidaCommand
{
    public int IdUsuario { get; init; }
    public DateTime FechaInicio { get; init; }
    public DateTime FechaFin { get; init; }
    public int[] ListaComidas { get; init; }
    public string[]? HorariosEntrega { get; init; }

    public UpdatePlanComidaCommand(int idUsuario, DateTime fechaInicio, DateTime fechaFin, int[] listaComidas, string[]? horariosEntrega = null)
    {
        if (idUsuario <= 0) throw new ArgumentException("IdUsuario must be greater than zero.", nameof(idUsuario));
        if (fechaInicio == default) throw new ArgumentException("FechaInicio is required.", nameof(fechaInicio));
        if (fechaFin <= fechaInicio) throw new ArgumentException("FechaFin must be after FechaInicio.", nameof(fechaFin));
        if (listaComidas is not { Length: 21 }) throw new ArgumentException("ListaComidas must contain exactly 21 meal ids.", nameof(listaComidas));

        IdUsuario = idUsuario;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        ListaComidas = listaComidas;
        HorariosEntrega = horariosEntrega;
    }
}
