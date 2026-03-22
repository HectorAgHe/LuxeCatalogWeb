using FluentValidation;
using LuxeCatalog.Business.DTOs.Auth;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequest> _validator;

    public AuthController(IAuthService authService, IValidator<LoginRequest> validator)
    {
        _authService = authService;
        _validator = validator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _authService.LoginAsync(request);
        if (result is null)
            return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo." });

        return Ok(result);
    }

    [HttpGet("me/{id}")]
    public async Task<IActionResult> GetMe(int id)
    {
        var result = await _authService.GetByIdAsync(id);
        if (result is null)
            return NotFound(new { message = "Usuario no encontrado." });

        return Ok(result);
    }
}