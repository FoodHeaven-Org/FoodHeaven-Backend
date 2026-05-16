using food_heaven_backend.Security.Domain.Comands;
using food_heaven_backend.Security.Domain.Exceptions;
using food_heaven_backend.Security.Domain.Service;
using food_heaven_backend.Security.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace food_heaven_backend.Security.Presentation.REST
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;

        // Constructor para inyectar el servicio de comandos y el servicio de consultas
        public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService)
        {
            _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
            _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            try
            {
                var user = await _userCommandService.Handle(command);
                return Created(string.Empty, user); // Devuelve el usuario creado
            }
            catch (UsernameAlreadyTakenException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var jwToken = await _userCommandService.Handle(command);
                return Ok(jwToken);
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        // Nuevo endpoint GET para obtener un usuario por ID
        [HttpGet("api/v1/User/{userId}")]
        [Authorize]  // Asegúrate de tener autorización adecuada aquí
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                // Consulta el usuario por ID
                var query = new GetUserByIdQuery(userId);
                var user = await _userQueryService.Handle(query);

                // Si el usuario no existe, se responde con un 404
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {userId} not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }
    }
}
