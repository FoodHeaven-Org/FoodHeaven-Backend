namespace food_heaven_backend.PlanComidas.Domain.Models.Exceptions;

public class NotPlanComidaFoundException : Exception
{
    public NotPlanComidaFoundException() : base("Plan de comida no encontrado.") { }
    public NotPlanComidaFoundException(string message) : base(message) { }
    public NotPlanComidaFoundException(string message, Exception inner) : base(message, inner) { }
}
