using LuxeCatalog.Business.DTOs.Brands;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET api/brands
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAllAsync();
            return Ok(result);
        }

        // GET api/brands/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _brandService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = "Marca no encontrada." });

            return Ok(result);
        }

        // POST api/brands
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BrandRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { message = "El nombre de la marca es requerido." });

            var result = await _brandService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT api/brands/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BrandRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { message = "El nombre de la marca es requerido." });

            var result = await _brandService.UpdateAsync(id, request);

            if (result is null)
                return NotFound(new { message = "Marca no encontrada." });

            return Ok(result);
        }

        // DELETE api/brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _brandService.DeleteAsync(id);

            if (!success)
                return NotFound(new { message = "Marca no encontrada." });

            return Ok(new { message = "Marca eliminada correctamente." });
        }
    }
}
