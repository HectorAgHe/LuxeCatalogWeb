using LuxeCatalog.Business.DTOs.Media;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using LuxeCatalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Implementations
{
    public class MediaService : IMediaService 
    {
        private readonly ApplicationDbContext _context;
        private const int Maxitems = 4;

        private MediaService(ApplicationDbContext context)
        {
            _context = context;
        }



        //============================================================================
        //============================================================================

        // Hero Images
        public async Task<List<HeroImageResponse>> GetHeroImagesAsync()
        {
            return await _context.HeroImages
                .OrderBy(h => h.Id)
                .Select(h => new HeroImageResponse
                {
                    Id = h.Id,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                }).ToListAsync();
        }
        public async Task<HeroImageResponse> AddHeroImageAsync(HeroImageRequest request)
        {
            var count = await _context.HeroImages.CountAsync();
            if (count >= Maxitems)
                throw new InvalidOperationException($"No puedes agregar mas de {Maxitems} elementos");

            var hero = new HeroImage
            {
                Title = request.Title,
                ImageUrl = request.ImageUrl,

            };

            _context.HeroImages.Add(hero);
            await _context.SaveChangesAsync();

            return new HeroImageResponse
            {
                Id = hero.Id,
                Title = hero.Title,
                ImageUrl = hero.ImageUrl
            };
        }

        public async Task<bool> DeleteHeroImageAsync(int id)
        {
            var hero = await _context.HeroImages.FindAsync(id);
            if(hero is  null) return false;

            _context.HeroImages.Remove(hero);
            await _context.SaveChangesAsync();

            return true; 
        }






        //============================================================================
        //============================================================================

        // Banner Images
        public async Task<List<BannerImageResponse>> GetBannersAsync()
        {
            return await _context.BannerImages
                .OrderBy(b => b.Id)
                .Select(b => new BannerImageResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl
                }).ToListAsync();            
        }
        public async Task<BannerImageResponse> AddBannerImageAsync(BannerImageRequest request)
        {
            var count = await _context.BannerImages.CountAsync();

            if (count >= Maxitems)
                throw new InvalidOperationException($"No puedes agrgas mas de {Maxitems} elementos banner");

            var banner = new BannerImage
            {
                Title = request.Title,
                ImageUrl = request.ImageUrl,
            };

            _context.BannerImages.Add(banner);
            await _context.SaveChangesAsync();

            return new BannerImageResponse
            {
                Id = banner.Id,
                Title = banner.Title,
                ImageUrl = banner.ImageUrl
            };
        }
      
        public async Task<bool> DeleteBannerImageAsync(int id)
        {
            var banner = await _context.BannerImages.FindAsync(id);
            if (banner == null) return false;

            _context.BannerImages.Remove(banner);
            await _context.SaveChangesAsync();

            return true;
        }





        //============================================================================
        //============================================================================

        // Videos
        public async Task<List<VideoResponse>> GetVideosAsync()
        {
            return await _context.Videos
                .OrderBy(v => v.Id)
                .Select(v => new VideoResponse
                {
                    Id = v.Id,
                    Title = v.Title,
                    YoutubeId = v.YoutubeId,
                }).ToListAsync();
        }
        public async Task<VideoResponse> AddVideAsync(VideoRequest request)
        {
            var count = await _context.Videos.CountAsync();
            if (count >= 4)
                throw new InvalidOperationException($"No es posible agregar mas de {Maxitems} elementos de video");

            var video = new Video
            {
                Title = request.Title,
                YoutubeId = request.YoutubeId,
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return new VideoResponse
            {
                Id = video.Id,
                Title = video.Title,
                YoutubeId = video.YoutubeId
            };
        }
        public async Task<bool> DeleteVideoAsync(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video is null) return false;

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
