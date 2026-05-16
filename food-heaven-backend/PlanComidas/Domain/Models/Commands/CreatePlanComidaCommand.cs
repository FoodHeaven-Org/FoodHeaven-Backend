namespace food_heaven_backend.PlanComidas.Domain.Models.Commands;

public record CreatePlanComidaCommand(
    int IdUsuario,
    DateTime FechaInicio,
    DateTime FechaFin,
    int[] ListaComidas
);
