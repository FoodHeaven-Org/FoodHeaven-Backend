using food_heaven_backend.PlanComidas.Domain.Models.Commands;
using food_heaven_backend.PlanComidas.Domain.Models.Entities;

namespace food_heaven_backend.PlanComidas.Domain.Services;

public interface IPlanComidaCommandService
{
    Task<PlanComida> Handle(CreatePlanComidaCommand command);
    Task<bool> Handle(UpdatePlanComidaCommand command, int id);
    Task<bool> Handle(DeletePlanComidaCommand command);
}
