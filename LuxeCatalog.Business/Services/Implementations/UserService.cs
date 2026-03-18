using Azure.Core;
using LuxeCatalog.Business.DTOs.Users;
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
    public class UserService :IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── Admin ──────────────────────────────────────────────

        public async Task<List<UserResponse>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.User)
                .OrderBy(u => u.FirstName)
                .Select(u => MapToResponse(u))
                .ToListAsync();
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user is null ? null : MapToResponse(user);
        }

        public async Task<UserResponse> CreateAsync(UserRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    string.IsNullOrEmpty(request.Password) ? "Luxe1234!" : request.Password
                ),
                Phone1 = request.Phone1,
                Phone2 = request.Phone2,
                CardNumber = request.CardNumber,
                ClvSocio = request.ClvSocio,
                Active = request.Active,
                PendingOrder = request.PendingOrder,
                Avatar = request.Avatar,
                Description = request.Description,
                Role = UserRole.User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        public async Task<UserResponse?> UpdateAsync(int id, UserRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return null;

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Phone1 = request.Phone1;
            user.Phone2 = request.Phone2;
            user.CardNumber = request.CardNumber;
            user.ClvSocio = request.ClvSocio;
            user.Active = request.Active;
            user.PendingOrder = request.PendingOrder;
            user.Avatar = request.Avatar;
            user.Description = request.Description;

            // Solo actualiza contraseña si se envió una nueva
            if (!string.IsNullOrEmpty(request.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;

            // No elimina si tiene pedidos
            bool hasOrders = await _context.Orders.AnyAsync(o => o.UserId == id);
            if (hasOrders) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // ── Cliente — perfil propio ────────────────────────────

        public async Task<UserResponse?> UpdateProfileAsync(int id, UpdateProfileRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return null;

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Phone1 = request.Phone1;
            user.Description = request.Description;
            user.Avatar = request.Avatar;

            // Solo actualiza contraseña si se envió una nueva
            if (!string.IsNullOrEmpty(request.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        // ── Direcciones ────────────────────────────────────────

        public async Task<List<AddressResponse>> GetAddressesAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenBy(a => a.Label)
                .Select(a => MapAddressToResponse(a))
                .ToListAsync();
        }

        public async Task<AddressResponse> AddAddressAsync(int userId, AddressRequest request)
        {
            // Si es default, quita el default de las demás
            if (request.IsDefault)
                await RemoveDefaultAsync(userId);

            var address = new Address
            {
                UserId = userId,
                Label = request.Label,
                Street = request.Street,
                ExtNumber = request.ExtNumber,
                IntNumber = request.IntNumber,
                Neighborhood = request.Neighborhood,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                References = request.References,
                IsDefault = request.IsDefault
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return MapAddressToResponse(address);
        }

        public async Task<AddressResponse?> UpdateAddressAsync(int userId, int addressId, AddressRequest request)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

            if (address is null) return null;

            // Si cambia a default, quita el default de las demás
            if (request.IsDefault && !address.IsDefault)
                await RemoveDefaultAsync(userId);

            address.Label = request.Label;
            address.Street = request.Street;
            address.ExtNumber = request.ExtNumber;
            address.IntNumber = request.IntNumber;
            address.Neighborhood = request.Neighborhood;
            address.City = request.City;
            address.State = request.State;
            address.ZipCode = request.ZipCode;
            address.References = request.References;
            address.IsDefault = request.IsDefault;

            await _context.SaveChangesAsync();

            return MapAddressToResponse(address);
        }

        public async Task<bool> DeleteAddressAsync(int userId, int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

            if (address is null) return false;

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return true;
        }

        // ── Privados ───────────────────────────────────────────

        private async Task RemoveDefaultAsync(int userId)
        {
            await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault)
                .ExecuteUpdateAsync(a => a.SetProperty(x => x.IsDefault, false));
        }

        private static UserResponse MapToResponse(User user) => new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone1 = user.Phone1,
            Phone2 = user.Phone2,
            CardNumber = user.CardNumber,
            ClvSocio = user.ClvSocio,
            Active = user.Active,
            PendingOrder = user.PendingOrder,
            Role = user.Role.ToString(),
            Avatar = user.Avatar,
            Description = user.Description
        };

        private static AddressResponse MapAddressToResponse(Address address) => new()
        {
            Id = address.Id,
            UserId = address.UserId,
            Label = address.Label,
            Street = address.Street,
            ExtNumber = address.ExtNumber,
            IntNumber = address.IntNumber,
            Neighborhood = address.Neighborhood,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            References = address.References,
            IsDefault = address.IsDefault
        };
    }
}
