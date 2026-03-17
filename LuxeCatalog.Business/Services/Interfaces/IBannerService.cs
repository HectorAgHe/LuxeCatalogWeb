using LuxeCatalog.Business.DTOs.Banners;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IBannerService
    {
        Task<List<BannerResponse>> GetAllAsync();
        Task<List<BannerResponse>> GetActiveSeasonAsync();
        Task<BannerResponse?> GetByIdAsync(int id);
        Task<BannerResponse> CreateAsync(BannerRequest request);
        Task<BannerResponse?> UpdateAsync(int id, BannerRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
