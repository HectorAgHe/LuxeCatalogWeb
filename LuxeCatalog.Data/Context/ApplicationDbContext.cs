using LuxeCatalog.Data.Entities;
using LuxeCatalog.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LuxeCatalog.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Catalog> Catalogs => Set<Catalog>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<HeroImage> HeroImages => Set<HeroImage>();
    public DbSet<BannerImage> BannerImages => Set<BannerImage>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Role).HasConversion<string>();
        });

        // ── Address ───────────────────────────────────────
        modelBuilder.Entity<Address>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Label).IsRequired().HasMaxLength(100);
            e.Property(a => a.Street).IsRequired().HasMaxLength(200);
            e.Property(a => a.State).IsRequired().HasMaxLength(100);
            e.HasOne(a => a.User)
             .WithMany(u => u.Addresses)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Season ────────────────────────────────────────
        modelBuilder.Entity<Season>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Value).IsRequired().HasMaxLength(100);
            e.HasIndex(s => s.Value).IsUnique();
            e.Property(s => s.Label).IsRequired().HasMaxLength(150);
        });

        // ── Catalog ───────────────────────────────────────
        modelBuilder.Entity<Catalog>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(200);
            e.HasOne(c => c.Season)
             .WithMany(s => s.Catalogs)
             .HasForeignKey(c => c.SeasonId)
             .OnDelete(DeleteBehavior.Restrict); // No eliminar temporada si tiene catálogos
        });

        // ── Brand ─────────────────────────────────────────
        modelBuilder.Entity<Brand>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Name).IsRequired().HasMaxLength(200);
        });

        // ── Banner ────────────────────────────────────────
        modelBuilder.Entity<Banner>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(200);
            e.HasOne(b => b.Season)
             .WithMany(s => s.Banners)
             .HasForeignKey(b => b.SeasonId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── HeroImage ─────────────────────────────────────
        modelBuilder.Entity<HeroImage>(e =>
        {
            e.HasKey(h => h.Id);
            e.Property(h => h.Title).IsRequired().HasMaxLength(200);
            e.Property(h => h.ImageUrl).IsRequired();
        });

        // ── BannerImage ───────────────────────────────────
        modelBuilder.Entity<BannerImage>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(200);
            e.Property(b => b.ImageUrl).IsRequired();
        });

        // ── Video ─────────────────────────────────────────
        modelBuilder.Entity<Video>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.Title).IsRequired().HasMaxLength(200);
            e.Property(v => v.YoutubeId).IsRequired().HasMaxLength(50);
        });

        // ── Order ─────────────────────────────────────────
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Status).HasConversion<string>();
            e.Property(o => o.Total).HasPrecision(18, 2);
            e.Property(o => o.AddressSnapshot).IsRequired();
            e.HasOne(o => o.User)
             .WithMany(u => u.Orders)
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.Restrict); // No eliminar usuario si tiene pedidos
        });

        // ── OrderProduct ──────────────────────────────────
        modelBuilder.Entity<OrderProduct>(e =>
        {
            e.HasKey(op => op.Id);
            e.Property(op => op.Quantity).IsRequired();
            e.HasOne(op => op.Order)
             .WithMany(o => o.Products)
             .HasForeignKey(op => op.OrderId)
             .OnDelete(DeleteBehavior.Cascade); // Si se elimina el pedido, se eliminan sus productos
        });
    }
}