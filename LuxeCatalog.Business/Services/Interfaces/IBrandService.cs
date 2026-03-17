using LuxeCatalog.Business.DTOs.Brands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandResponse>> GetAllAsync();
        Task<BrandResponse?> GetByIdAsync(int id);
        Task<BrandResponse> CreateAsync(BrandRequest request);
        Task<BrandResponse?> UpdateAsync(int id, BrandRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
