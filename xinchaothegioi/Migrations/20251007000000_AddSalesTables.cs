namespace xinchaothegioi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalesTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        KhachHangId = c.Int(nullable: false, identity: true),
                        Ten = c.String(nullable: false, maxLength: 100),
                        SoDienThoai = c.String(nullable: false, maxLength: 20),
                        GioiTinh = c.String(maxLength: 10),
                        KhuVuc = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.KhachHangId)
                .Index(t => t.SoDienThoai, unique: true, name: "IX_KhachHang_SDT");
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        HoaDonId = c.Int(nullable: false, identity: true),
                        KhachHangId = c.Int(nullable: false),
                        NgayMua = c.DateTime(nullable: false),
                        SoTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TenPhim = c.String(maxLength: 200),
                        DanhSachGhe = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.HoaDonId)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangId)
                .Index(t => t.KhachHangId);
            
            CreateTable(
                "dbo.ChiTietHoaDons",
                c => new
                    {
                        ChiTietHoaDonId = c.Int(nullable: false, identity: true),
                        HoaDonId = c.Int(nullable: false),
                        SoGhe = c.String(nullable: false, maxLength: 10),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ChiTietHoaDonId)
                .ForeignKey("dbo.HoaDons", t => t.HoaDonId, cascadeDelete: true)
                .Index(t => t.HoaDonId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "HoaDonId", "dbo.HoaDons");
            DropForeignKey("dbo.HoaDons", "KhachHangId", "dbo.KhachHangs");
            DropIndex("dbo.ChiTietHoaDons", new[] { "HoaDonId" });
            DropIndex("dbo.HoaDons", new[] { "KhachHangId" });
            DropIndex("dbo.KhachHangs", "IX_KhachHang_SDT");
            DropTable("dbo.ChiTietHoaDons");
            DropTable("dbo.HoaDons");
            DropTable("dbo.KhachHangs");
        }
    }
}
