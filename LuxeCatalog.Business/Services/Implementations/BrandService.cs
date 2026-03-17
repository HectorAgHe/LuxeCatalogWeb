using LuxeCatalog.Business.DTOs.Brands;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using LuxeCatalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BrandResponse>> GetAllAsync()
        {
            return await _context.Brands
            .OrderBy(b => b.Name)
            .Select(b => MapToResponse(b))
            .ToListAsync();
        }
        public async Task<BrandResponse?> GetByIdAsync(int id)
        {
            var brand = await _context.Brands
                .FindAsync(id);
            return brand is null ? null : MapToResponse(brand);
        }

        public async Task<BrandResponse> CreateAsync(BrandRequest request)
        {
            var brand = new Brand
            {
                Name = request.Name,
                Logo = request.Logo,
                Description = request.Description
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            
            return MapToResponse(brand);
        }

        public async Task<BrandResponse?> UpdateAsync(int id, BrandRequest request)
        {
            var brand = await _context.Brands
                .FindAsync(id);
            if (brand is null) return null;

            brand.Name = request.Name;
            brand.Logo = request.Logo;
            brand.Description = request.Description;

            await _context.SaveChangesAsync();
            return MapToResponse(brand);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _context.Brands
                .FindAsync(id);
            if(brand is null) return false;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return true;
        }


        private static BrandResponse MapToResponse(Brand brand) => new()
        {
            Id = brand.Id,
            Name = brand.Name,
            Logo = brand.Logo,
            Description = brand.Description
        };
    }
}
