using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Users
{
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? Phone1 { get; set; }
        public string? Description { get; set; }
        public string? Password { get; set; } // null = no cambiar
        public string? Avatar { get; set; }
    }
}
