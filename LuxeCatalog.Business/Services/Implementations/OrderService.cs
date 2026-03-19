using LuxeCatalog.Business.DTOs.Orders;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using LuxeCatalog.Data.Entities;
using LuxeCatalog.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── Admin ──────────────────────────────────────────────

        public async Task<List<OrderResponse>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                .OrderByDescending(o => o.Date)
                .Select(o => MapToResponse(o))
                .ToListAsync();
        }

        public async Task<List<OrderResponse>> GetByStatusAsync(string status)
        {
            // Parsea el string a enum de forma segura
            if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
                return [];

            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                .Where(o => o.Status == orderStatus)
                .OrderByDescending(o => o.Date)
                .Select(o => MapToResponse(o))
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusRequest request)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null) return false;

            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
                return false;

            var previousStatus = order.Status;
            order.Status = newStatus;

            // Actualiza PendingOrder del usuario según el estado
            if (newStatus == OrderStatus.Entregado || newStatus == OrderStatus.SinExistencia)
                order.User.PendingOrder = false;
            else if (newStatus == OrderStatus.Pendiente || newStatus == OrderStatus.Adquirido)
                order.User.PendingOrder = true;

            await _context.SaveChangesAsync();

            return true;
        }

        // ── Cliente ────────────────────────────────────────────

        public async Task<List<OrderResponse>> GetByUserAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Date)
                .Select(o => MapToResponse(o))
                .ToListAsync();
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order is null ? null : MapToResponse(order);
        }

        public async Task<OrderResponse> CreateAsync(OrderRequest request)
        {
            var order = new Order
            {
                UserId = request.UserId,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Pendiente,
                AddressSnapshot = request.AddressSnapshot,
                PaymentMethod = request.PaymentMethod,
                Notes = request.Notes,
                VoucherUrl = request.VoucherUrl,
                Total = 0,
                Products = request.Products.Select(p => new OrderProduct
                {
                    Code = p.Code,
                    CatalogName = p.CatalogName,
                    CatalogNumber = p.CatalogNumber,
                    Description = p.Description,
                    Size = p.Size,
                    Color = p.Color,
                    Quantity = p.Quantity,
                    Alternative = p.Alternative
                }).ToList()
            };

            _context.Orders.Add(order);

            // Marca al usuario con pedido pendiente
            var user = await _context.Users.FindAsync(request.UserId);
            if (user is not null)
                user.PendingOrder = true;

            await _context.SaveChangesAsync();

            // Recarga con relaciones para el mapeo
            await _context.Entry(order).Reference(o => o.User).LoadAsync();

            return MapToResponse(order);
        }

        // ── Privados ───────────────────────────────────────────

        private static OrderResponse MapToResponse(Order order) => new()
        {
            Id = order.Id,
            UserId = order.UserId,
            UserFullName = $"{order.User?.FirstName} {order.User?.LastName}".Trim(),
            UserEmail = order.User?.Email ?? string.Empty,
            UserClvSocio = order.User?.ClvSocio,
            UserPhone = order.User?.Phone1,
            UserAvatar = order.User?.Avatar,
            Date = order.Date,
            Status = order.Status.ToString(),
            AddressSnapshot = order.AddressSnapshot,
            PaymentMethod = order.PaymentMethod,
            Notes = order.Notes,
            Total = order.Total,
            VoucherUrl = order.VoucherUrl,
            Products = order.Products?.Select(p => new OrderProductResponse
            {
                Id = p.Id,
                Code = p.Code,
                CatalogName = p.CatalogName,
                CatalogNumber = p.CatalogNumber,
                Description = p.Description,
                Size = p.Size,
                Color = p.Color,
                Quantity = p.Quantity,
                Alternative = p.Alternative
            }).ToList() ?? []
        };
    }
}
