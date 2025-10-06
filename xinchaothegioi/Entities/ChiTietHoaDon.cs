using System.ComponentModel.DataAnnotations;

namespace xinchaothegioi.Entities
{
    public class ChiTietHoaDon
    {
        public int ChiTietHoaDonId { get; set; }

        public int HoaDonId { get; set; }

        [Required]
        [MaxLength(10)]
        public string SoGhe { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public virtual HoaDon HoaDon { get; set; }
    }
}
