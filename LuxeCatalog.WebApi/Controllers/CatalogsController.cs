using LuxeCatalog.Business.DTOs.Catalogs;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // GET api/catalogs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _catalogService.GetAllAsync();
            return Ok(result);
        }

        // GET api/catalogs/visible
        [HttpGet("visible")]
        public async Task<IActionResult> GetVisible()
        {
            var result = await _catalogService.GetVisibleAsync();
            return Ok(result);
        }

        // GET api/catalogs/visible-cliente
        [HttpGet("visible-cliente")]
        public async Task<IActionResult> GetVisibleCliente()
        {
            var result = await _catalogService.GetVisibleClienteAsync();
            return Ok(result);
        }

        // GET api/catalogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _catalogService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = "Catálogo no encontrado." });

            return Ok(result);
        }

        // POST api/catalogs
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CatalogRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { message = "El nombre del catálogo es requerido." });

            if (request.SeasonId <= 0)
                return BadRequest(new { message = "Debes seleccionar una temporada." });

            var result = await _catalogService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT api/catalogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CatalogRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { message = "El nombre del catálogo es requerido." });

            var result = await _catalogService.UpdateAsync(id, request);

            if (result is null)
                return NotFound(new { message = "Catálogo no encontrado." });

            return Ok(result);
        }

        // DELETE api/catalogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _catalogService.DeleteAsync(id);

            if (!success)
                return NotFound(new { message = "Catálogo no encontrado." });

            return Ok(new { message = "Catálogo eliminado correctamente." });
        }
    }
}
