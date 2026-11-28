using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _31241026329_TranHuuTuanBang.Models;

namespace _31241026329_TranHuuTuanBang.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<HoaDon> HoaDons => Set<HoaDon>();
    public DbSet<ChiTietHoaDon> ChiTietHoaDons => Set<ChiTietHoaDon>();
    public DbSet<ShippingDetail> ShippingDetails => Set<ShippingDetail>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<Genre>().HasIndex(x => x.Name).IsUnique();
        model.Entity<Movie>().Property(x => x.Title).IsRequired().HasMaxLength(200);
        model.Entity<Movie>().Property(x => x.Summary).IsRequired().HasMaxLength(2000);
        model.Entity<Movie>().Property(x => x.PicturePath).HasMaxLength(200);
        model.Entity<Movie>().Property(x => x.Price).HasPrecision(18, 2);
        model.Entity<Movie>().HasOne(x => x.GenreObj).WithMany(x => x.Movies)
            .HasForeignKey(x => x.GenreID).OnDelete(DeleteBehavior.Restrict);
        model.Entity<HoaDon>().Property(x => x.TongTien).HasPrecision(18, 2);
        model.Entity<HoaDon>().HasIndex(x => x.CheckoutToken).IsUnique();
        model.Entity<ChiTietHoaDon>().Property(x => x.DonGia).HasPrecision(18, 2);
        model.Entity<ChiTietHoaDon>().Property(x => x.TenPhim).IsRequired().HasMaxLength(200);
        model.Entity<ChiTietHoaDon>().HasOne(x => x.HoaDonObj).WithMany(x => x.ChiTietHoaDons)
            .HasForeignKey(x => x.MaHoaDon);
        model.Entity<ChiTietHoaDon>().HasOne(x => x.MovieObj).WithMany()
            .HasForeignKey(x => x.MaMovie).OnDelete(DeleteBehavior.Restrict);
        model.Entity<HoaDon>().HasOne(x => x.ShippingDetail).WithOne()
            .HasForeignKey<ShippingDetail>(x => x.HoaDonID);
    }
}
