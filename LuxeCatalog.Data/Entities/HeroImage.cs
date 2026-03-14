using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Data.Entities
{
    public class HeroImage
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
