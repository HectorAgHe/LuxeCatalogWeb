using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Media
{
    public class VideoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string YoutubeId { get; set; } = string.Empty;
    }
}
