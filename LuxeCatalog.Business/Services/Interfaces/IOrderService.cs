using LuxeCatalog.Business.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IOrderService
    {
        // Admin
        Task<List<OrderResponse>> GetAllAsync();
        Task<List<OrderResponse>> GetByStatusAsync(string status);
        Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusRequest request);

        // Cliente
        Task<List<OrderResponse>> GetByUserAsync(int userId);
        Task<OrderResponse?> GetByIdAsync(int id);
        Task<OrderResponse> CreateAsync(OrderRequest request);
    }
}

