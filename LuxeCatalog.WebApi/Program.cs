using LuxeCatalog.Data.Context;
using Microsoft.EntityFrameworkCore;
using LuxeCatalog.Business.Services.Implementations;
using LuxeCatalog.Business.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios ──────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS — permite peticiones desde Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// ── Services ───────────────────────────────────────────
#region Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBannerService, BannerService>();
#endregion




// ── Pipeline ───────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();