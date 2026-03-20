using LuxeCatalog.Business.Services.Implementations;
using LuxeCatalog.Business.Services.Interfaces;
using LuxeCatalog.Business.Settings;
using LuxeCatalog.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios ──────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
#endregion

// ── Pipeline ───────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();