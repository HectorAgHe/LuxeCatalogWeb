using LuxeCatalog.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuxeCatalog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IStorageService _storageService;

        private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private static readonly string[] ImageAndPdfExtensions = [".jpg", ".jpeg", ".png", ".webp", ".pdf"];


        // 500MB para PDFs grandes
        private const long MaxFileSize = 500 * 1024 * 1024; // 500MB

        public UploadController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        // POST api/upload/catalogo
        [HttpPost("catalogo")]
        [RequestSizeLimit(500 * 1024 * 1024)] // solo en UploadCatalogo
        public async Task<IActionResult> UploadCatalogo(IFormFile file)
        {
            return await HandleUpload(file, "catalogos", ImageAndPdfExtensions);
        }

        // POST api/upload/marca
        [HttpPost("marca")]
        [RequestSizeLimit(10 * 1024 * 1024)] 
        public async Task<IActionResult> UploadMarca(IFormFile file)
        {
            return await HandleUpload(file, "marcas", ImageExtensions);
        }

        // POST api/upload/hero
        [HttpPost("hero")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadHero(IFormFile file)
        {
            return await HandleUpload(file, "hero", ImageExtensions);
        }

        // POST api/upload/banner
        [HttpPost("banner")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadBanner(IFormFile file)
        {
            return await HandleUpload(file, "banners", ImageExtensions);
        }

        // POST api/upload/avatar
        [HttpPost("avatar")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            return await HandleUpload(file, "avatars", ImageExtensions);
        }

        // DELETE api/upload
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return BadRequest(new { message = "La URL del archivo es requerida." });

            var success = await _storageService.DeleteAsync(fileUrl);

            if (!success)
                return NotFound(new { message = "Archivo no encontrado." });

            return Ok(new { message = "Archivo eliminado correctamente." });
        }

        // ── Método privado reutilizable ────────────────────────
        private async Task<IActionResult> HandleUpload(IFormFile file, string folder, string[] allowedExtensions)
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { message = "No se recibió ningún archivo." });

            if (file.Length > MaxFileSize)
                return BadRequest(new { message = "El archivo supera el límite permitido." });

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = $"Tipo de archivo no permitido. Permitidos: {string.Join(", ", allowedExtensions)}" });

            // Nombre único: "catalogos_20260320_a3f9b2c1.pdf"
            var uniqueName = $"{folder}_{DateTime.UtcNow:yyyyMMdd}_{Guid.NewGuid().ToString()[..8]}{extension}";

            using var stream = file.OpenReadStream();
            var url = await _storageService.UploadAsync(stream, uniqueName, file.ContentType, folder);

            return Ok(new
            {
                url,
                fileName = uniqueName,
                size = file.Length,
                extension
            });
        }

    }
}
