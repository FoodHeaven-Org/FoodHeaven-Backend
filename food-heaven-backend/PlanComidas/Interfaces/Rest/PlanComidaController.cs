using System.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using food_heaven_backend.PlanComidas.Application.Internal.CommandServices;
using food_heaven_backend.PlanComidas.Application.Internal.QueryServices;
using food_heaven_backend.PlanComidas.Domain.Model.Commands;
using food_heaven_backend.PlanComidas.Domain.Model.Queries;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.PlanComidas.Interfaces.Rest.Resources;
using food_heaven_backend.PlanComidas.Interfaces.Rest.Transform;

namespace food_heaven_backend.PlanComidas.Interfaces.Rest;

[Route("api/v1/[controller]")]
[ApiController]
[Produces("application/json")]
public class PlanComidaController(
    IPlanComidaQueryService queryService,
    IPlanComidaCommandService commandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlanComidaResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var result = await queryService.Handle(new GetAllPlanComidasQuery());
        return result.Any()
            ? Ok(result.Select(PlanComidaResourceFromEntityAssembler.ToResourceFromEntity))
            : NotFound("No se encontraron planes.");
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IEnumerable<PlanComidaResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await queryService.Handle(new GetPlanComidaByUserIdQuery(id));
        return result.Any()
            ? Ok(PlanComidaResourceFromEntityAssembler.ToResourcesFromEntities(result))
            : NotFound($"No se encontró ningún plan con el UserID {id}.");
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlanComidaResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await commandService.Handle(new DeletePlanComidaCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
