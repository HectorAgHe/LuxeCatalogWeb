using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Brands
{
    public class BrandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo {  get; set; }
        public string? Description { get; set; }
    }
}
