using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string? UserClvSocio { get; set; }
        public string? UserPhone { get; set; }
        public string? UserAvatar { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AddressSnapshot { get; set; } = "{}";
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public decimal Total { get; set; }
        public string? VoucherUrl { get; set; }
        public List<OrderProductResponse> Products { get; set; } = [];
    }
}
