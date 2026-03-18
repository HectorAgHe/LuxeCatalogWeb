using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Users
{
    public class AddressRequest
    {
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
    }
}
