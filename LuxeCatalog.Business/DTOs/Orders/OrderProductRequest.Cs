using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Orders
{
    public class OrderProductRequest
    {
        public string? Code { get; set; }
        public string? CatalogName { get; set; }
        public string? CatalogNumber { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Alternative { get; set; }
    }
}
