using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Seasons
{
    public class SeasonResponse
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Description {  get; set; } = string.Empty;
        public bool IsActive { get; set; } 
    }
}
