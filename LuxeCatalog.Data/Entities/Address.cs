using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string ExtNumber { get; set; } = string.Empty;
        public string? IntNumber { get; set; }
        public string Neighborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string? References { get; set; }
        public bool IsDefault { get; set; } = false;

        // Navegación
        public User User { get; set; } = null!;
    }
}
