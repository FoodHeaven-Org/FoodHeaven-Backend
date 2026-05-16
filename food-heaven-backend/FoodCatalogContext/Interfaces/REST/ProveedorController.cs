using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Exceptions;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;
using food_heaven_backend.FoodCatalogContext.Interfaces.REST.Transform;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProveedorController(IProveedorQueryService queryService, IProveedorCommandService commandService) : ControllerBase
    {
        private readonly IProveedorQueryService _queryService = queryService;
        private readonly IProveedorCommandService _commandService = commandService;

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _queryService.Handle(new GetAllProvidersQuery());
            return result.Any()
                ? Ok(result.Select(ProveedorResourceFromEntityAssembler.ToResourceFromEntity))
                : NotFound("No providers found.");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Invalid provider ID.");

            var result = await _queryService.Handle(new GetProviderByIdQuery(id));
            return result != null
                ? Ok(ProveedorResourceFromEntityAssembler.ToResourceFromEntity(result))
                : NotFound($"Provider with ID {id} not found.");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Post([FromBody] CreateProveedorCommand command)
        {
            if (command == null) return BadRequest("Provider data cannot be null.");

            try
            {
                await _commandService.Handle(command);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProveedorCommand command)
        {
            if (id <= 0) return BadRequest("Invalid provider ID.");

            try
            {
                await _commandService.Handle(command, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid provider ID.");

            try
            {
                await _commandService.Handle(new DeleteProveedorCommand(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
