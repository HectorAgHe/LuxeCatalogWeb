using FluentValidation;
using LuxeCatalog.Business.DTOs.Brands;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _brandService;
    private readonly IValidator<BrandRequest> _validator;

    public BrandsController(IBrandService brandService, IValidator<BrandRequest> validator)
    {
        _brandService = brandService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _brandService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _brandService.GetByIdAsync(id);
        if (result is null)
            return NotFound(new { message = "Marca no encontrada." });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BrandRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _brandService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BrandRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _brandService.UpdateAsync(id, request);
        if (result is null)
            return NotFound(new { message = "Marca no encontrada." });

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _brandService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Marca no encontrada." });

        return Ok(new { message = "Marca eliminada correctamente." });
    }
}