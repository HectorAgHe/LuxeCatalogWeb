using FluentValidation;
using LuxeCatalog.Business.DTOs.Banners;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannersController : ControllerBase
{
    private readonly IBannerService _bannerService;
    private readonly IValidator<BannerRequest> _validator;

    public BannersController(IBannerService bannerService, IValidator<BannerRequest> validator)
    {
        _bannerService = bannerService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bannerService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveSeason()
    {
        var result = await _bannerService.GetActiveSeasonAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bannerService.GetByIdAsync(id);
        if (result is null)
            return NotFound(new { message = "Banner no encontrado." });

        return Ok(result);
    }

    // Solo Admin
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BannerRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _bannerService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Solo Admin
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BannerRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _bannerService.UpdateAsync(id, request);
        if (result is null)
            return NotFound(new { message = "Banner no encontrado." });

        return Ok(result);
    }


    // Solo Admin
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _bannerService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Banner no encontrado." });

        return Ok(new { message = "Banner eliminado correctamente." });
    }
}