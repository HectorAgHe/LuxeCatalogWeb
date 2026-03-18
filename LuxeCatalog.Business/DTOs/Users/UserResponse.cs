using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Users
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? CardNumber { get; set; }
        public string? ClvSocio { get; set; }
        public bool Active { get; set; }
        public bool PendingOrder { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Description { get; set; }
    }
}
