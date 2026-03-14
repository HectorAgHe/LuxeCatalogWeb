using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Data.Entities
{
    public class Season
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty; // slug ej: "spring-summer-2025"
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = false;

        // Navegación
        public ICollection<Catalog> Catalogs { get; set; } = [];
        public ICollection<Banner> Banners { get; set; } = [];
    }
}
