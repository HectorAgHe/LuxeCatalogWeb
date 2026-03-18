using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Media
{
    public class VideoRequest
    {
        public string Title { get; set; } = string.Empty;
        public string YoutubeId { get; set; } = string.Empty;
    }
}
