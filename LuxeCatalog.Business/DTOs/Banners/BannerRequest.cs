using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Banners
{
    public class BannerRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Image { get; set; }
        public string? Cta { get; set; }
        public int SeasonId { get; set; }
    }
    
}
