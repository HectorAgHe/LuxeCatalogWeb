using LuxeCatalog.Business.DTOs.Auth;


namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> GetByIdAsync(int id);
    }
}
