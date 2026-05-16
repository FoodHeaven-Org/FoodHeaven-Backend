using System.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using food_heaven_backend.PlanComidas.Application.CommandServices;
using food_heaven_backend.PlanComidas.Application.QueryServices;
using food_heaven_backend.PlanComidas.Domain.Models.Commands;
using food_heaven_backend.PlanComidas.Domain.Models.Queries;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.PlanComidas.Interfaces.REST.Transform;

namespace food_heaven_backend.PlanComidas.Interfaces.REST;

[Route("api/v1/[controller]")]
[ApiController]
[Produces("application/json")]
public class PlanComidaController(
    IPlanComidaQueryService queryService,
    IPlanComidaCommandService commandService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await queryService.Handle(new GetAllPlanComidasQuery());
        return result.Any()
            ? Ok(result.Select(PlanComidaResourceFromEntityAssembler.ToResourceFromEntity))
            : NotFound("No se encontraron planes.");
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await queryService.Handle(new GetPlanComidaByUserIdQuery(id));
        return result.Any()
            ? Ok(PlanComidaResourceFromEntityAssembler.ToResourcesFromEntities(result))
            : NotFound($"No se encontró ningún plan con el UserID {id}.");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePlanComidaCommand command)
    {
        try
        {
            var plan = await commandService.Handle(command);
            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, PlanComidaResourceFromEntityAssembler.ToResourceFromEntity(plan));
        }
        catch (ValidationException ex) { return UnprocessableEntity(ex.Message); }
        catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdatePlanComidaCommand command)
    {
        try
        {
            var updated = await commandService.Handle(command, id);
            return updated ? Ok() : NotFound();
        }
        catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await commandService.Handle(new DeletePlanComidaCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
