using FluentValidation;
using LuxeCatalog.Business.DTOs.Users;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<UserRequest> _userValidator;
    private readonly IValidator<UpdateProfileRequest> _profileValidator;
    private readonly IValidator<AddressRequest> _addressValidator;

    public UsersController(
        IUserService userService,
        IValidator<UserRequest> userValidator,
        IValidator<UpdateProfileRequest> profileValidator,
        IValidator<AddressRequest> addressValidator)
    {
        _userService = userService;
        _userValidator = userValidator;
        _profileValidator = profileValidator;
        _addressValidator = addressValidator;
    }

    // Solo Admin — lista todos los usuarios
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    // Solo Admin — busca usuario por ID
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetByIdAsync(id);
        if (result is null)
            return NotFound(new { message = "Usuario no encontrado." });

        return Ok(result);
    }

    // Solo Admin — crea usuario
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRequest request)
    {
        var validation = await _userValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _userService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Solo Admin — edita usuario completo
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserRequest request)
    {
        var validation = await _userValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _userService.UpdateAsync(id, request);
        if (result is null)
            return NotFound(new { message = "Usuario no encontrado." });

        return Ok(result);
    }

    // Cliente — edita su propio perfil
    [Authorize]
    [HttpPut("{id}/profile")]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
    {
        var validation = await _profileValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _userService.UpdateProfileAsync(id, request);
        if (result is null)
            return NotFound(new { message = "Usuario no encontrado." });

        return Ok(result);
    }

    // Solo Admin — elimina usuario
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _userService.DeleteAsync(id);
        if (!success)
            return BadRequest(new { message = "No se puede eliminar. El usuario tiene pedidos registrados." });

        return Ok(new { message = "Usuario eliminado correctamente." });
    }

    // Cliente — obtiene sus direcciones
    [Authorize]
    [HttpGet("{userId}/addresses")]
    public async Task<IActionResult> GetAddresses(int userId)
    {
        var result = await _userService.GetAddressesAsync(userId);
        return Ok(result);
    }

    // Cliente — agrega dirección
    [Authorize]
    [HttpPost("{userId}/addresses")]
    public async Task<IActionResult> AddAddress(int userId, [FromBody] AddressRequest request)
    {
        var validation = await _addressValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _userService.AddAddressAsync(userId, request);
        return Ok(result);
    }

    // Cliente — edita dirección
    [Authorize]
    [HttpPut("{userId}/addresses/{addressId}")]
    public async Task<IActionResult> UpdateAddress(int userId, int addressId, [FromBody] AddressRequest request)
    {
        var validation = await _addressValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _userService.UpdateAddressAsync(userId, addressId, request);
        if (result is null)
            return NotFound(new { message = "Dirección no encontrada." });

        return Ok(result);
    }

    // Cliente — elimina dirección
    [Authorize]
    [HttpDelete("{userId}/addresses/{addressId}")]
    public async Task<IActionResult> DeleteAddress(int userId, int addressId)
    {
        var success = await _userService.DeleteAddressAsync(userId, addressId);
        if (!success)
            return NotFound(new { message = "Dirección no encontrada." });

        return Ok(new { message = "Dirección eliminada correctamente." });
    }
}