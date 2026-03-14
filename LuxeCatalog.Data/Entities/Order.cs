using LuxeCatalog.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pendiente;
        public string AddressSnapshot { get; set; } = "{}"; // JSON de la dirección al momento del pedido
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public decimal Total { get; set; } = 0;
        public string? VoucherUrl { get; set; }

        // Navegación
        public User User { get; set; } = null!;
        public ICollection<OrderProduct> Products { get; set; } = [];
    }
}
