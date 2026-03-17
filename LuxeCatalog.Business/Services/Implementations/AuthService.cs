using LuxeCatalog.Business.DTOs.Auth;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        //Inyeccion de dependencia
        private readonly ApplicationDbContext _context;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<AuthResponse?> LoginAsyc(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Active);

            if (user is null) return null;

            // Verifica la contraseña contra el hash guardado
            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!validPassword) return null;

            return MapToResponse(user);
        }
        public async Task<AuthResponse?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return null;
            return MapToResponse(user);
        }

        private static AuthResponse MapToResponse(LuxeCatalog.Data.Entities.User user) => new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Avatar = user.Avatar,
            ClvSocio = user.ClvSocio,
            Active = user.Active
        };

    }
}
