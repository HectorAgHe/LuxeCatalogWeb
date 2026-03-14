using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Data.Entities
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Pages { get; set; }
        public string? CoverImage { get; set; }
        public string? PdfUrl { get; set; }
        public bool Visible { get; set; } = true;
        public bool VisibleCliente { get; set; } = true;
        public int SeasonId { get; set; }

        // Navegación
        public Season Season { get; set; } = null!;
    }
}
