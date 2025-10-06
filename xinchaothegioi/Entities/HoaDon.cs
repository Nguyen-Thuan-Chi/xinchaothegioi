using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xinchaothegioi.Entities
{
    public class HoaDon
    {
        public int HoaDonId { get; set; }

        public int KhachHangId { get; set; }

        [Required]
        public DateTime NgayMua { get; set; }

        [Required]
        public decimal SoTien { get; set; }

        [MaxLength(200)]
        public string TenPhim { get; set; }

        [MaxLength(100)]
        public string DanhSachGhe { get; set; } // v? d?: "1, 2, 3"

        public virtual KhachHang KhachHang { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}