using LuxeCatalog.Business.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?>LoginAsync(LoginRequest request);
        Task<AuthResponse?>GetByIdAsync(int id);

    }
}
