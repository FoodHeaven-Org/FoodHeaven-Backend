using food_heaven_backend.Security.Domain.Model.Commands;
using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.Exceptions;
using food_heaven_backend.Security.Domain.Model.Queries;
using food_heaven_backend.Security.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace food_heaven_backend.Security.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly IUserQueryService _userQueryService;

    public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        try
        {
            var user = await _userCommandService.Handle(command);
            return Created(string.Empty, user);
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
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var jwtToken = await _userCommandService.Handle(command);
            return Ok(jwtToken);
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

    [HttpGet("{userId:int}")]
    [Authorize]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var user = await _userQueryService.Handle(new GetUserByIdQuery(userId));
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
        }
    }
}