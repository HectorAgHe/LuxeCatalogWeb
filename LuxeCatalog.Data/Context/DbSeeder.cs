using LuxeCatalog.Data.Entities;
using LuxeCatalog.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LuxeCatalog.Data.Context;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Solo ejecuta si no hay datos
        if (await context.Users.AnyAsync()) return;

        // ── Usuario Admin ──────────────────────────────────
        var admin = new User
        {
            FirstName = "Admin",
            LastName = "LuxeCatalog",
            Email = "admin@luxecatalog.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),
            Role = UserRole.Admin,
            Active = true,
            Phone1 = "5500000000",
            ClvSocio = "ADMIN001"
        };

        // ── Usuario Cliente de prueba ──────────────────────
        var cliente = new User
        {
            FirstName = "Cliente",
            LastName = "Prueba",
            Email = "cliente@luxecatalog.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Cliente1234!"),
            Role = UserRole.User,
            Active = true,
            Phone1 = "5511111111",
            ClvSocio = "CLI001"
        };

        context.Users.AddRange(admin, cliente);

        // ── Temporada inicial ──────────────────────────────
        var season = new Season
        {
            Value = "spring-summer-2026",
            Label = "Spring Summer 2026",
            Description = "Temporada primavera verano 2026",
            IsActive = true
        };

        context.Seasons.Add(season);

        await context.SaveChangesAsync();
    }
}