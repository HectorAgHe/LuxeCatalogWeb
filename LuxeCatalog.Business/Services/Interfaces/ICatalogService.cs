using LuxeCatalog.Business.DTOs.Catalogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CatalogResponse>> GetAllAsync();
        Task<List<CatalogResponse>> GetVisibleAsync();
        Task<List<CatalogResponse>> GetVisibleClienteAsync();
        Task<CatalogResponse?> GetByIdAsync(int id);
        Task<CatalogResponse> CreateAsync(CatalogRequest request);
        Task<CatalogResponse?> UpdateAsync(int id, CatalogRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
