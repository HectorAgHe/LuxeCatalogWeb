using LuxeCatalog.Data.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LuxeCatalog.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? CardNumber { get; set; }
        public string? ClvSocio { get; set; }
        public bool PendingOrder { get; set; } = false;
        public bool Active { get; set; } = true;
        public UserRole Role { get; set; } = UserRole.User;
        public string? Avatar { get; set; }
        public string? Description { get; set; }

        // Navegación
        public ICollection<Address> Addresses { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
    }
}
