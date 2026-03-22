using FluentValidation;
using LuxeCatalog.Business.DTOs.Media;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    private readonly IValidator<HeroImageRequest> _heroValidator;
    private readonly IValidator<BannerImageRequest> _bannerImageValidator;
    private readonly IValidator<VideoRequest> _videoValidator;

    public MediaController(
        IMediaService mediaService,
        IValidator<HeroImageRequest> heroValidator,
        IValidator<BannerImageRequest> bannerImageValidator,
        IValidator<VideoRequest> videoValidator)
    {
        _mediaService = mediaService;
        _heroValidator = heroValidator;
        _bannerImageValidator = bannerImageValidator;
        _videoValidator = videoValidator;
    }

    [HttpGet("hero")]
    public async Task<IActionResult> GetHeroImages()
    {
        var result = await _mediaService.GetHeroImagesAsync();
        return Ok(result);
    }

    [HttpPost("hero")]
    public async Task<IActionResult> AddHeroImage([FromBody] HeroImageRequest request)
    {
        var validation = await _heroValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        try
        {
            var result = await _mediaService.AddHeroImageAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("hero/{id}")]
    public async Task<IActionResult> DeleteHeroImage(int id)
    {
        var success = await _mediaService.DeleteHeroImageAsync(id);
        if (!success)
            return NotFound(new { message = "Imagen no encontrada." });

        return Ok(new { message = "Imagen eliminada correctamente." });
    }

    [HttpGet("banners")]
    public async Task<IActionResult> GetBannerImages()
    {
        var result = await _mediaService.GetBannerImagesAsync();
        return Ok(result);
    }

    [HttpPost("banners")]
    public async Task<IActionResult> AddBannerImage([FromBody] BannerImageRequest request)
    {
        var validation = await _bannerImageValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        try
        {
            var result = await _mediaService.AddBannerImageAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("banners/{id}")]
    public async Task<IActionResult> DeleteBannerImage(int id)
    {
        var success = await _mediaService.DeleteBannerImageAsync(id);
        if (!success)
            return NotFound(new { message = "Imagen no encontrada." });

        return Ok(new { message = "Imagen eliminada correctamente." });
    }

    [HttpGet("videos")]
    public async Task<IActionResult> GetVideos()
    {
        var result = await _mediaService.GetVideosAsync();
        return Ok(result);
    }

    [HttpPost("videos")]
    public async Task<IActionResult> AddVideo([FromBody] VideoRequest request)
    {
        var validation = await _videoValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        try
        {
            var result = await _mediaService.AddVideoAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("videos/{id}")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var success = await _mediaService.DeleteVideoAsync(id);
        if (!success)
            return NotFound(new { message = "Video no encontrado." });

        return Ok(new { message = "Video eliminado correctamente." });
    }
}