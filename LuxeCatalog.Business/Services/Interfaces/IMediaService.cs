using LuxeCatalog.Business.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IMediaService
    {
        // Hero Images
        Task<List<HeroImageResponse>> GetHeroImagesAsync();
        Task<HeroImageResponse> AddHeroImageAsync(HeroImageRequest request);
        Task<bool> DeleteHeroImageAsync(int id);

        // Banner Images
        Task<List<BannerImageResponse>> GetBannersAsync();
        Task<BannerImageResponse> AddBannerImageAsync(BannerImageRequest request);
        Task<bool> DeleteBannerImageAsync(int id);

        // Videos
        Task<List<VideoResponse>> GetVideosAsync();
        Task<VideoResponse> AddVideAsync(VideoRequest request);
        Task<bool> DeleteVideoAsync(int id);
    }
}
