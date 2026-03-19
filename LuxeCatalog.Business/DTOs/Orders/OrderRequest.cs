using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Orders
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public string AddressSnapshot { get; set; } = "{}";
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public string? VoucherUrl { get; set; }
        public List<OrderProductRequest> Products { get; set; } = [];
    }
}
