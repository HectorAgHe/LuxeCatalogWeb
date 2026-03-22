using FluentValidation;
using LuxeCatalog.Business.DTOs.Seasons;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeasonsController : ControllerBase
{
    private readonly ISeasonService _seasonService;
    private readonly IValidator<SeasonRequest> _validator;

    public SeasonsController(ISeasonService seasonService, IValidator<SeasonRequest> validator)
    {
        _seasonService = seasonService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _seasonService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _seasonService.GetActiveAsync();
        if (result is null)
            return NotFound(new { message = "No hay temporada activa." });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SeasonRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _seasonService.CreateAsync(request);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var success = await _seasonService.ActivateAsync(id);
        if (!success)
            return NotFound(new { message = "Temporada no encontrada." });

        return Ok(new { message = "Temporada activada correctamente." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _seasonService.DeleteAsync(id);
        if (!success)
            return BadRequest(new { message = "No se puede eliminar. La temporada está activa o tiene catálogos/banners asignados." });

        return Ok(new { message = "Temporada eliminada correctamente." });
    }
}