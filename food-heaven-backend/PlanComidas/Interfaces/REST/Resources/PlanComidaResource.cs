namespace food_heaven_backend.PlanComidas.Interfaces.REST.Resources;

public record PlanComidaResource(
    int Id,
    int IdUsuario,
    DateTime FechaInicio,
    DateTime FechaFin,
    int[] ListaComidas
);
