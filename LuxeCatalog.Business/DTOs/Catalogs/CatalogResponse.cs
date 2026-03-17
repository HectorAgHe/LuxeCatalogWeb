using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Catalogs
{
    public class CatalogResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Pages { get; set; }
        public string? CoverImage { get; set; }
        public string? PdfUrl { get; set; }
        public bool Visible { get; set; }
        public bool VisibleCliente { get; set; }
        public int SeasonId { get; set; }
        public string SeasonLabel { get; set; } = string.Empty;
    }
}
