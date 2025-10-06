using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xinchaothegioi.Entities
{
    public class KhachHang
    {
        public int KhachHangId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        [Required]
        [MaxLength(20)]
        public string SoDienThoai { get; set; }

        [MaxLength(10)]
        public string GioiTinh { get; set; } // "Nam" / "N?"

        [MaxLength(100)]
        public string KhuVuc { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}