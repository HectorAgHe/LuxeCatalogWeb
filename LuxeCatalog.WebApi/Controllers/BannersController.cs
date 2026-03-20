using LuxeCatalog.Business.DTOs.Banners;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;

        }
        // Get api/banners
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bannerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bannerService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(new { message = "Banner no encontrado." });
            }
            return Ok(result);

        }

        // POST /api/banners
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BannerRequest request)
        {
            if (string.IsNullOrEmpty(request.Title))
                return BadRequest(new { mesaage = "El titulo del banner es requerido." });

            if (request.SeasonId <= 0)
                return BadRequest(new { message = "Debes seleccionar una temporada" });

            var result = await _bannerService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT api/banners/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BannerRequest request)
        {
            if (string.IsNullOrEmpty(request.Title))
                return BadRequest(new { message = "El titulo del banner es requerido" });


            var result = await _bannerService.UpdateAsync(id, request);

            if (result is null)
                return NotFound(new { message = "Banner no encontrado" });

            return Ok(result);
        }


        // DELETE api/banners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var success = await _bannerService.DeleteAsync(id);

            if (!success)
                return NotFound(new { message = "Banner no encontrado" });

            return Ok(new { message = "Banner eliminado correctamente" });
        }
        



    }
}
