using FluentValidation;
using LuxeCatalog.Business.Services.Implementations;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Business.Settings;
using LuxeCatalog.Business.Validators;
using LuxeCatalog.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios ──────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// OpenAPI nativo .NET 10 + Scalar
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "LuxeCatalog API";
        document.Info.Version = "v1";
        document.Info.Description = "API REST para el sistema de catálogo de calzado LuxeCatalog";
        return Task.CompletedTask;
    });
});

// EF Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Cloudflare R2
builder.Services.Configure<CloudflareSettings>(
    builder.Configuration.GetSection("Cloudflare"));

// Límite de subida 500MB
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500 * 1024 * 1024;
});

//-----------------------------------------------------------------------
//                        JWT
//--------------------------------------------------------------------
// JWT Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ClockSkew = TimeSpan.Zero // sin margen de tiempo extra
    };
});

builder.Services.AddAuthorization();

//-------------------------------------------------------------------------
// -----------------------------------------------------------------------

// FluentValidation — registra todos los validators de Business
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

#region Business Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ITokenService, TokenService>();
#endregion

// ── Pipeline ───────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "LuxeCatalog API";
        options.Theme = ScalarTheme.Purple;
        options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.Fetch);
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ── Seed de datos ──────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
    await DbSeeder.SeedAsync(context);
}

app.Run();