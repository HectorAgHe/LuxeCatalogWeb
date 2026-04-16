using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Media
{
    public class PresignRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Folder { get; set; } = string.Empty; // "catalogos", "marcas", etc.
    }
}
