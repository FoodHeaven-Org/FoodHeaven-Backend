using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.FoodCatalogContext.Interfaces.REST.Transform;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Queries;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Exceptions;

namespace food_heaven_backend.FoodCatalogContext.Interfaces.REST
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
        public async Task<IActionResult> GetAsync([FromQuery(Name = "id_tipo_comida")] int? idTipoComida)
        {
            if (idTipoComida.HasValue && idTipoComida > 0)
            {
                var comidas = await _comidaQueryService.Handle(new GetComidaByIdTipoComida(idTipoComida.Value));
                return comidas.Any()
                    ? Ok(comidas.Select(ComidaResourceFromEntityAssembler.ToResourceFromEntity))
                    : NotFound("No se encontraron comidas para ese tipo.");
            }

            var all = await _comidaQueryService.Handle(new GetAllComidaQuery());
            return all.Any()
                ? Ok(all.Select(ComidaResourceFromEntityAssembler.ToResourceFromEntity))
                : NotFound("No comidas found.");
        }

        // GET: api/Comida/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("Invalid provider ID.");

            var result = await _comidaQueryService.Handle(new GetComidaByIdQuery(id));
            return result != null 
                ? Ok(ComidaResourceFromEntityAssembler.ToResourceFromEntity(result)) 
                : NotFound($"Comida with id '{id}' not found.");
        }


        // POST: api/Comida
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
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
        public async Task<IActionResult> Put(int id, [FromBody] UpdateComidaCommand command)
        {
            if (id <= 0) return BadRequest("Invalid comida ID.");

            try
            {
                await _comidaCommandService.Handle(command, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Comida/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid comida ID.");

            try
            {
                await _comidaCommandService.Handle(new DeleteComidaCommand(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        
        
    }
    
}
