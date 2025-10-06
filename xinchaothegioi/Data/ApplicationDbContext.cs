using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using xinchaothegioi.Entities;
using xinchaothegioi.Models;

namespace xinchaothegioi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            // Use migrations instead of recreating database
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Users_Username") { IsUnique = true }));

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedDate)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .IsRequired();

            modelBuilder.Entity<User>()
                .ToTable("Users");

            // KhachHang
            modelBuilder.Entity<KhachHang>()
                .HasKey(k => k.KhachHangId);

            modelBuilder.Entity<KhachHang>()
                .Property(k => k.Ten)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<KhachHang>()
                .Property(k => k.SoDienThoai)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_KhachHang_SDT") { IsUnique = true }));

            modelBuilder.Entity<KhachHang>()
                .Property(k => k.GioiTinh)
                .HasMaxLength(10);

            modelBuilder.Entity<KhachHang>()
                .Property(k => k.KhuVuc)
                .HasMaxLength(100);

            // HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasKey(h => h.HoaDonId);

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.NgayMua)
                .IsRequired();

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.SoTien)
                .IsRequired()
                .HasPrecision(18, 2);

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.TenPhim)
                .HasMaxLength(200);

            modelBuilder.Entity<HoaDon>()
                .Property(h => h.DanhSachGhe)
                .HasMaxLength(100);

            modelBuilder.Entity<HoaDon>()
                .HasRequired(h => h.KhachHang)
                .WithMany(k => k.HoaDons)
                .HasForeignKey(h => h.KhachHangId)
                .WillCascadeOnDelete(false);

            // ChiTietHoaDon
            modelBuilder.Entity<ChiTietHoaDon>()
                .HasKey(ct => ct.ChiTietHoaDonId);

            modelBuilder.Entity<ChiTietHoaDon>()
                .Property(ct => ct.SoGhe)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<ChiTietHoaDon>()
                .Property(ct => ct.DonGia)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ChiTietHoaDon>()
                .HasRequired(ct => ct.HoaDon)
                .WithMany(h => h.ChiTietHoaDons)
                .HasForeignKey(ct => ct.HoaDonId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}