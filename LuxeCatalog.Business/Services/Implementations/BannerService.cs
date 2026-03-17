using LuxeCatalog.Business.DTOs.Banners;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using LuxeCatalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Implementations
{
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _context;

        public BannerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BannerResponse>> GetAllAsync()
        {
            return await _context.Banners
                .Include(b => b.Season)
                .OrderBy(b => b.Title)
                .Select(b => MapToResponse(b))
                .ToListAsync();
        }

        public async Task<List<BannerResponse>> GetActiveSeasonAsync()
        {
            // Solo banners de la temporada activa para el home público
            return await _context.Banners
                .Include(b => b.Season)
                .Where(b => b.Season.IsActive)
                .OrderBy(b => b.Title)
                .Select(b => MapToResponse(b))
                .ToListAsync();
        }

        public async Task<BannerResponse?> GetByIdAsync(int id)
        {
            var banner = await _context.Banners
                .Include(b => b.Season)
                .FirstOrDefaultAsync(b => b.Id == id);

            return banner is null ? null : MapToResponse(banner);
        }

        public async Task<BannerResponse> CreateAsync(BannerRequest request)
        {
            var banner = new Banner
            {
                Title = request.Title,
                Subtitle = request.Subtitle,
                Image = request.Image,
                Cta = request.Cta,
                SeasonId = request.SeasonId
            };

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            await _context.Entry(banner).Reference(b => b.Season).LoadAsync();

            return MapToResponse(banner);
        }

        public async Task<BannerResponse?> UpdateAsync(int id, BannerRequest request)
        {
            var banner = await _context.Banners
                .Include(b => b.Season)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (banner is null) return null;

            banner.Title = request.Title;
            banner.Subtitle = request.Subtitle;
            banner.Image = request.Image;
            banner.Cta = request.Cta;
            banner.SeasonId = request.SeasonId;

            await _context.SaveChangesAsync();

            await _context.Entry(banner).Reference(b => b.Season).LoadAsync();

            return MapToResponse(banner);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner is null) return false;

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return true;
        }

        private static BannerResponse MapToResponse(Banner banner) => new()
        {
            Id = banner.Id,
            Title = banner.Title,
            Subtitle = banner.Subtitle,
            Image = banner.Image,
            Cta = banner.Cta,
            SeasonId = banner.SeasonId,
            SeasonLabel = banner.Season?.Label ?? string.Empty
        };
    }
}
