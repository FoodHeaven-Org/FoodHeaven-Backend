using System.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using food_heaven_backend.PlanComidas.Domain.Model.Commands;
using food_heaven_backend.PlanComidas.Domain.Model.Queries;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.PlanComidas.Interfaces.Rest.Resources;
using food_heaven_backend.Shared.Interfaces.Rest;
using Microsoft.AspNetCore.Authorization;

namespace food_heaven_backend.PlanComidas.Interfaces.Rest;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
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
        var currentUserId = User.GetAuthenticatedUserId();
        var result = await queryService.Handle(new GetPlanComidaByUserIdQuery(currentUserId));
        return result.Any()
            ? Ok(result.Select(PlanComidaResource.FromEntity))
            : NotFound("No meal plans were found.");
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IEnumerable<PlanComidaResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var currentUserId = User.GetAuthenticatedUserId();
        if (id != currentUserId) return Forbid();

        var result = await queryService.Handle(new GetPlanComidaByUserIdQuery(id));
        return result.Any()
            ? Ok(result.Select(PlanComidaResource.FromEntity))
            : NotFound($"No meal plan was found for user {id}.");
    }

    [HttpPost]
    [ProducesResponseType(typeof(PlanComidaResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CreatePlanComidaCommand command)
    {
        try
        {
            if (command.IdUsuario != User.GetAuthenticatedUserId()) return Forbid();

            var plan = await commandService.Handle(command);
            return CreatedAtAction(nameof(GetById), new { id = plan.IdUsuario }, PlanComidaResource.FromEntity(plan));
        }
        catch (ValidationException ex) { return UnprocessableEntity(ex.Message); }
        catch (ArgumentException ex) { return UnprocessableEntity(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdatePlanComidaCommand command)
    {
        try
        {
            if (command.IdUsuario != User.GetAuthenticatedUserId()) return Forbid();

            var updated = await commandService.Handle(command, id);
            return updated ? Ok() : NotFound();
        }
        catch (DataException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return UnprocessableEntity(ex.Message); }
        catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await commandService.Handle(new DeletePlanComidaCommand(id), User.GetAuthenticatedUserId());
        return deleted ? NoContent() : NotFound();
    }
}
