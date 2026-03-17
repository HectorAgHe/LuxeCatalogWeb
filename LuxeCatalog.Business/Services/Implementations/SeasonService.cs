using LuxeCatalog.Business.DTOs.Seasons;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using LuxeCatalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Net;
{
    
}

namespace LuxeCatalog.Business.Services.Implementations
{
    public class SeasonService : ISeasonService
    {

        public readonly ApplicationDbContext _context;
        public SeasonService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<List<SeasonResponse>> GetAllAsync()
        {
            return await _context.Seasons
                .OrderByDescending(s => s.IsActive)
                .ThenBy(s => s.Label)
                .Select(s => MapToResponse(s))
                .ToListAsync();
        }


        public async Task<SeasonResponse?> GetActiveAsync()
        {
            var season = await _context.Seasons
                .FirstOrDefaultAsync(s => s.IsActive);
            return season is null ? null : MapToResponse(season)
        }
        public async Task<SeasonResponse> CreateAsync(SeasonRequest request)
        {
            //Gener el slug automaticamente desde el label
            // ejemplo: "spring summer 2026" -> "Spring-summer-2025"

            var value = request.Label
                .ToLower()
                .Trim()
                .Replace(" ", "-");

            var season = new Season
            {
                Value = value,
                Label = request.Label,
                Description = request.Description,
                IsActive = false          // nueva temporada nunca está activa por defecto
            };


            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();

            return MapToResponse(season);
        }


        public async Task<bool> ActivateAync(int id)
        {
            // Busca la temporada a activar
            var season = await _context.Seasons
                .FindAsync(id);

            if (season is null) return false;
            //Descativa todas las temporadas primero
            await _context.Seasons
                .Where(s => s.IsActive)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsActive, false));


            //ExecuteUpdateAsync es una feature moderna de EF Core que ejecuta un UPDATE directo en SQL sin cargar las entidades en memoria: UPDATE Seasons SET IsActive = 0 WHERE IsActive = 1
            //¿Por qué primero desactivar todas? Porque el sistema solo puede tener una temporada activa. Si activas la nueva sin desactivar la anterior, tendrías dos activas.



            // Paso 2 — activa la seleccionada
            season.IsActive = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var season = await _context.Seasons
                .FindAsync(id);
            if (season is null) return false;

            //No permite eliminar la temporada activa
            if (season.IsActive) return false;

            //No permite eliminar si tiene catalogos o banners asociados
            bool hasRelated = await _context.Catalogs
                .AnyAsync(c => c.SeasonId == id) 
                || await _context.Banners
                .AnyAsync(b => b.SeasonId == id);

            if (hasRelated) return false;

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();

            return true;
        }
// AnyAsync — genera SELECT TOP 1 en SQL, muy eficiente, no carga todos los registros
// El || es cortocircuito — si ya encontró catálogos, ni consulta los banners

        private static SeasonResponse MapToResponse(Season season) => new()
        {
            Id = season.Id,
            Value = season.Value,
            Label = season.Label,
            Description = season.Description,
            IsActive = season.IsActive,
        };

    }
}
