namespace food_heaven_backend.PlanComidas.Domain.Model.Commands;

public record UpdatePlanComidaCommand(
    int IdUsuario,
    DateTime FechaInicio,
    DateTime FechaFin,
    int[] ListaComidas

);
