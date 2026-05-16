using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.FoodCatalogContext.Interfaces.Rest.Resources;

using food_heaven_backend.FoodCatalogContext.Domain.Model.Queries;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Exceptions;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.Rest
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ComidaController(IComidaQueryService comidaQueryService, IComidaCommandService comidaCommandService) : ControllerBase
    {
        private readonly IComidaQueryService _comidaQueryService = comidaQueryService;
        private readonly IComidaCommandService _comidaCommandService = comidaCommandService;

        // GET: api/Comida or api/Comida?id_tipo_comida=1
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ComidaResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromQuery(Name = "id_tipo_comida")] int? idTipoComida)
        {
            if (idTipoComida.HasValue && idTipoComida > 0)
            {
                var comidas = await _comidaQueryService.Handle(new GetComidasByTipoComidaQuery(idTipoComida.Value));
                return comidas.Any()
                    ? Ok(comidas.Select(ComidaResource.FromEntity))
                    : NotFound("No se encontraron comidas para ese tipo.");
            }

            var all = await _comidaQueryService.Handle(new GetAllComidaQuery());
            return all.Any()
                ? Ok(all.Select(ComidaResource.FromEntity))
                : NotFound("No comidas found.");
        }

        // GET: api/Comida/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ComidaResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Invalid provider ID.");

            var result = await _comidaQueryService.Handle(new GetComidaByIdQuery(id));
            return result != null 
                ? Ok(ComidaResource.FromEntity(result)) 
                : NotFound($"Comida with id '{id}' not found.");
        }


        // POST: api/Comida
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateComidaCommand command)
        {
            if (command == null) return BadRequest("Comida data cannot be null.");

            try
            {
                await _comidaCommandService.Handle(command);
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
            catch (DuplicateNameException)
            {
                return Conflict("A comida with the same name already exists.");
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Comida/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateComidaCommand command)
        {
            if (id <= 0) return BadRequest("Invalid comida ID.");

            try
            {
                await _comidaCommandService.Handle(command, id);
                return Ok();
            }
            catch (DataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Comida/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid comida ID.");

            try
            {
                var deleted = await _comidaCommandService.Handle(new DeleteComidaCommand(id));
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        
        
    }
    
}
