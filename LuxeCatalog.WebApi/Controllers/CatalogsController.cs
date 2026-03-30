using FluentValidation;
using LuxeCatalog.Business.DTOs.Catalogs;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogsController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IValidator<CatalogRequest> _validator;

    public CatalogsController(ICatalogService catalogService, IValidator<CatalogRequest> validator)
    {
        _catalogService = catalogService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _catalogService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("visible")]
    public async Task<IActionResult> GetVisible()
    {
        var result = await _catalogService.GetVisibleAsync();
        return Ok(result);
    }

    [HttpGet("visible-cliente")]
    public async Task<IActionResult> GetVisibleCliente()
    {
        var result = await _catalogService.GetVisibleClienteAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _catalogService.GetByIdAsync(id);
        if (result is null)
            return NotFound(new { message = "Catálogo no encontrado." });

        return Ok(result);
    }

    // Solo Admin
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CatalogRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _catalogService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Solo Admin
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CatalogRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _catalogService.UpdateAsync(id, request);
        if (result is null)
            return NotFound(new { message = "Catálogo no encontrado." });

        return Ok(result);
    }


    // Solo Admin
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _catalogService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Catálogo no encontrado." });

        return Ok(new { message = "Catálogo eliminado correctamente." });
    }
}