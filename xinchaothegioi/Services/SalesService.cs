using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using xinchaothegioi.Data;
using xinchaothegioi.Entities;

namespace xinchaothegioi.Services
{
    public class SalesService : IDisposable
    {
        private readonly ApplicationDbContext _db;

        public SalesService()
        {
            _db = new ApplicationDbContext();
        }

        public IEnumerable<HoaDon> GetInvoices()
        {
            return _db.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons)
                .OrderByDescending(h => h.NgayMua)
                .ToList();
        }

        public KhachHang GetOrCreateCustomer(string name, string phone, string gender, string region)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("T?n b?t bu?c", nameof(name));
            if (!IsValidPhoneNumber(phone)) throw new ArgumentException("S?T kh?ng h?p l?", nameof(phone));

            var kh = _db.KhachHangs.FirstOrDefault(x => x.SoDienThoai == phone);
            if (kh == null)
            {
                kh = new KhachHang
                {
                    Ten = name.Trim(),
                    SoDienThoai = phone.Trim(),
                    GioiTinh = string.IsNullOrWhiteSpace(gender) ? null : gender.Trim(),
                    KhuVuc = string.IsNullOrWhiteSpace(region) ? null : region.Trim()
                };
                _db.KhachHangs.Add(kh);
                _db.SaveChanges();
            }
            else
            {
                // update basic fields if changed
                kh.Ten = name.Trim();
                kh.GioiTinh = string.IsNullOrWhiteSpace(gender) ? kh.GioiTinh : gender.Trim();
                kh.KhuVuc = string.IsNullOrWhiteSpace(region) ? kh.KhuVuc : region.Trim();
                _db.SaveChanges();
            }
            return kh;
        }

        public HoaDon CreateInvoice(KhachHang kh, string movieTitle, IEnumerable<string> seats, DateTime date, Func<int, int> seatPriceResolver)
        {
            if (kh == null) throw new ArgumentNullException(nameof(kh));
            if (seats == null || !seats.Any()) throw new ArgumentException("Ch?a ch?n gh?", nameof(seats));

            var seatList = seats
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .OrderBy(s => int.Parse(s))
                .ToList();

            decimal total = 0;
            var details = new List<ChiTietHoaDon>();
            foreach (var s in seatList)
            {
                int sn = int.Parse(s);
                var price = seatPriceResolver(sn);
                total += price;
                details.Add(new ChiTietHoaDon
                {
                    SoGhe = s,
                    SoLuong = 1,
                    DonGia = price
                });
            }

            var invoice = new HoaDon
            {
                KhachHangId = kh.KhachHangId,
                NgayMua = date,
                SoTien = total,
                TenPhim = movieTitle,
                DanhSachGhe = string.Join(", ", seatList),
                ChiTietHoaDons = details
            };

            _db.HoaDons.Add(invoice);
            _db.SaveChanges();
            return invoice;
        }

        public bool IsSeatPurchasedInRegion(string region, string seatNumber)
        {
            if (string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(seatNumber)) return false;

            // Compare exact seat numbers within comma-separated list
            return _db.HoaDons
                .Include(h => h.KhachHang)
                .Where(h => h.KhachHang.KhuVuc == region && h.DanhSachGhe != null)
                .AsEnumerable()
                .Any(h => h.DanhSachGhe
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Any(s => string.Equals(s, seatNumber, StringComparison.Ordinal)));
        }

        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            phone = phone.Replace(" ", "").Replace("-", "");
            if (phone.Length < 10 || phone.Length > 11) return false;
            if (!phone.StartsWith("0")) return false;
            return phone.All(char.IsDigit);
        }

        public void DeleteInvoices(IEnumerable<int> ids)
        {
            if (ids == null) return;
            var idSet = new HashSet<int>(ids);
            var invoices = _db.HoaDons.Include(h => h.ChiTietHoaDons).Where(h => idSet.Contains(h.HoaDonId)).ToList();
            if (invoices.Count == 0) return;

            // Remove details first if necessary (cascade may handle this)
            foreach (var inv in invoices)
            {
                if (inv.ChiTietHoaDons != null)
                {
                    _db.Set<ChiTietHoaDon>().RemoveRange(inv.ChiTietHoaDons);
                }
                _db.HoaDons.Remove(inv);
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
