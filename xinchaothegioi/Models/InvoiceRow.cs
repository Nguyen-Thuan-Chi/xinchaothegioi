using System;

namespace xinchaothegioi.Models
{
    // DTO d?ng ?? bind DataGridView theo c?t c? ??nh
    public class InvoiceRow
    {
        public string MaHoaDon { get; set; }
        public string TenKhachHang { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string KhuVuc { get; set; }
        public string TenPhim { get; set; }
        public string DanhSachGhe { get; set; }
        public int SoLuongVe { get; set; }
        public string ThanhTienHienThi { get; set; }
        public DateTime NgayBan { get; set; }
    }
}
