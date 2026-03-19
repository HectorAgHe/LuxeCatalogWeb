using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.DTOs.Orders
{
    public class UpdateOrderStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}
