using LuxeCatalog.Business.DTOs.Catalogs;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using LuxeCatalog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuxeCatalog.Business.Services.Implementations;

public class CatalogService : ICatalogService
{
    private readonly ApplicationDbContext _context;

    public CatalogService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CatalogResponse>> GetAllAsync()
    {
        return await _context.Catalogs
            .Include(c => c.Season)
            .OrderBy(c => c.Name)
            .Select(c => MapToResponse(c))
            .ToListAsync();
    }

    public async Task<List<CatalogResponse>> GetVisibleAsync()
    {
        // Solo los visibles en home público filtrados por temporada activa
        return await _context.Catalogs
            .Include(c => c.Season)
            .Where(c => c.Visible && c.Season.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => MapToResponse(c))
            .ToListAsync();
    }

    public async Task<List<CatalogResponse>> GetVisibleClienteAsync()
    {
        // Solo los visibles para usuarios logueados filtrados por temporada activa
        return await _context.Catalogs
            .Include(c => c.Season)
            .Where(c => c.VisibleCliente && c.Season.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => MapToResponse(c))
            .ToListAsync();
    }

    public async Task<CatalogResponse?> GetByIdAsync(int id)
    {
        var catalog = await _context.Catalogs
            .Include(c => c.Season)
            .FirstOrDefaultAsync(c => c.Id == id);

        return catalog is null ? null : MapToResponse(catalog);
    }

    public async Task<CatalogResponse> CreateAsync(CatalogRequest request)
    {
        var catalog = new Catalog
        {
            Name = request.Name,
            Category = request.Category,
            Pages = request.Pages,
            CoverImage = request.CoverImage,
            PdfUrl = request.PdfUrl,
            Visible = request.Visible,
            VisibleCliente = request.VisibleCliente,
            SeasonId = request.SeasonId
        };

        _context.Catalogs.Add(catalog);
        await _context.SaveChangesAsync();

        // Recarga con Season incluida para el mapeo
        await _context.Entry(catalog).Reference(c => c.Season).LoadAsync();

        return MapToResponse(catalog);
    }

    public async Task<CatalogResponse?> UpdateAsync(int id, CatalogRequest request)
    {
        var catalog = await _context.Catalogs
            .Include(c => c.Season)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (catalog is null) return null;

        catalog.Name = request.Name;
        catalog.Category = request.Category;
        catalog.Pages = request.Pages;
        catalog.CoverImage = request.CoverImage;
        catalog.PdfUrl = request.PdfUrl;
        catalog.Visible = request.Visible;
        catalog.VisibleCliente = request.VisibleCliente;
        catalog.SeasonId = request.SeasonId;

        await _context.SaveChangesAsync();

        // Recarga Season si cambió
        await _context.Entry(catalog).Reference(c => c.Season).LoadAsync();

        return MapToResponse(catalog);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var catalog = await _context.Catalogs.FindAsync(id);
        if (catalog is null) return false;

        _context.Catalogs.Remove(catalog);
        await _context.SaveChangesAsync();

        return true;
    }

    private static CatalogResponse MapToResponse(Catalog catalog) => new()
    {
        Id = catalog.Id,
        Name = catalog.Name,
        Category = catalog.Category,
        Pages = catalog.Pages,
        CoverImage = catalog.CoverImage,
        PdfUrl = catalog.PdfUrl,
        Visible = catalog.Visible,
        VisibleCliente = catalog.VisibleCliente,
        SeasonId = catalog.SeasonId,
        SeasonLabel = catalog.Season?.Label ?? string.Empty
    };
}