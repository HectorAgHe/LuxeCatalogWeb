using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Banners
{
    public class BannerResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Image { get; set; }
        public string? Cta { get; set; }
        public int SeasonId { get; set; }
        public string SeasonLabel { get; set; } = string.Empty;
    }
}
