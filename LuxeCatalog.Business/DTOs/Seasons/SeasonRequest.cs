using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Seasons
{
    public class SeasonRequest
    {
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
