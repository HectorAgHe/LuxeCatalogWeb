using LuxeCatalog.Business.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IUserService
    {
        // Admin
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(UserRequest request);
        Task<UserResponse?> UpdateAsync(int id, UserRequest request);
        Task<bool> DeleteAsync(int id);

        // User

        Task<UserResponse?> UpdateProfileAsync(int id, UpdateProfileRequest request);

        //Direcciones

        Task<List<AddressResponse>> GetAddressesAsync(int userId);
        Task<AddressResponse> AddAddressAsync(int userId, AddressRequest request);
        Task<AddressResponse?> UpdateAddressAsync(int userId, int addressId, AddressRequest request);
        Task<bool> DeleteAddressAsync(int userId, int addressId);
    }
}
