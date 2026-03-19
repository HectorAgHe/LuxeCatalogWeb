using LuxeCatalog.Business.DTOs.Auth;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// Authenticates a user based on the provided login credentials and returns an authentication result.
        /// </summary>
        /// <remarks>This method is intended for use in HTTP POST requests to the 'login' endpoint. The
        /// response varies depending on the validity of the credentials and user status.</remarks>
        /// <param name="request">The login request containing the user's email and password. Cannot be null. Both fields are required for
        /// authentication.</param>
        /// <returns>An IActionResult containing the authentication result. Returns 400 Bad Request if required fields are
        /// missing, 401 Unauthorized if credentials are invalid or the user is inactive, or 200 OK with the
        /// authentication result on success.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest( new {message = "Email y contraseña son requeridos"});

            var result = await _authService.LoginAsyc(request);

            if (result is null)
                return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo" });

            return Ok(result);
        }



        /// <summary>
        /// Retrieves the authenticated user information for the specified user identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose information is to be retrieved. Must be a valid user ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user information if found; otherwise, a NotFound result.</returns>
        [HttpGet("me/{id}")]
        public async Task<IActionResult> GetMe(int id)
        {
            var result = await _authService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { mesage = "Usuario no encontrado" });

            return Ok(result);
        }




    }
}
