using LuxeCatalog.Business.DTOs.Seasons;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<List<SeasonResponse>> GetAllAsync();
        Task<SeasonResponse?> GetActiveAsync();
        Task<SeasonResponse> CreateAsync(SeasonRequest request);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeleteAsync(int id);

    }
}
  