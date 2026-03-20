using LuxeCatalog.Business.DTOs.Media;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        // ── Hero Images ────────────────────────────────────────

        // GET api/media/hero
        [HttpGet("hero")]
        public async Task<IActionResult> GetHeroImages()
        {
            var result = _mediaService.GetHeroImagesAsync();
            return Ok(result);
        }



        // POST api/media/hero
        [HttpPost("hero")]
        public async Task<IActionResult> AddHeroImage([FromBody] HeroImageRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
                return BadRequest(new { message = "La url de la imagen es requerida" });

            try
            {
                var result = await _mediaService.AddHeroImageAsync(request);
                return Ok(result);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }


        // DELETE api/media/hero/5

        [HttpDelete("hero/{id}")]
        public async Task<IActionResult> DeleteHeroImage(int id)
        {
            var success = await _mediaService.DeleteHeroImageAsync(id);
            if (!success)
                return NotFound(new { message = "Imagen no encontrada" });

            return Ok(new {message = "Imagen eliminada correctamente"});
        }

        // ----Banner Images--------------------------------

        // GET api/media/banners

        [HttpGet("banners")]
        public async Task<IActionResult> GetAll()
        {
            var result = _mediaService.GetBannersAsync();

            return Ok(result);
        }

        // POST api/media/banners
        [HttpPost("banners")]
        public async Task<IActionResult> AddBannerImage(BannerImageRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
                return BadRequest(new { message = "La URL de imagen es requerida" });

            try
            {
                var result = await _mediaService.AddBannerImageAsync(request);
                    return Ok(result);

            }catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // DELETE api/media/banners/5
        [HttpDelete("banners/{id}")]
        public async Task<IActionResult> DeleteBannerImage(int id)
        {
            var success = await _mediaService.DeleteBannerImageAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Imagen no encontrada" });
            }
            return Ok(new { message = "Imagen eliminada exitosamente" });
        
        }


        //----- Videos-------------------------------------

        // GET api/media/videos

        [HttpGet("videos")]
        public async Task<IActionResult> GetVideos()
        {
            var result = await _mediaService.GetVideosAsync();
            return Ok(result);
        }

        [HttpPost("videos")]
        public async Task<IActionResult> AddVideo([FromBody] VideoRequest request)
        {
            if(string.IsNullOrEmpty(request.YoutubeId))
                return BadRequest(new { message = "El ID de Youtube es requerido"});

            try
            {
                var result = await _mediaService.AddVideAsync(request);
                return Ok(result);
            }catch (InvalidOperationException ex)
            {
                return BadRequest(new { mmessage = ex.Message});
            }

        }


        // DELETE api/media/videos/5
        [HttpDelete("videos/{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var success = await _mediaService.DeleteVideoAsync(id);

            if (!success)
                return NotFound(new { message = "Video no encontrado" });

            return Ok(new { message = "Video eliminado correctamente pibe" });
        }
    }
}
