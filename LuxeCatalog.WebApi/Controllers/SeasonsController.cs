using LuxeCatalog.Business.DTOs.Seasons;
using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonsController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }


        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var result = await _seasonService.GetAllAsync();
            return Ok(result);
        }

        // POST api/seasons
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SeasonRequest request)
        {
            if (string.IsNullOrEmpty(request.Label))
                return BadRequest(new { message = "El nombre de la temporada es requerido" });

            var result =_seasonService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }

        // PUT api/seasons/5/activate
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var success = await _seasonService.ActivateAsync(id);
            if (!success)
                return NotFound(new { message = "Temporada no encontrada" });
            
            return Ok(new { message = "Temporada activada correctamente" });

        }

        // DELETE api/seasons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _seasonService.DeleteAsync(id);

            if(!success)
                return BadRequest(new { message = "No se puede eliminar. La temporada esta activa o tiene catálogos/banners asignados." });
            return Ok(new { message = "Temporada eliminada correctamente." });
        }



    }
}
