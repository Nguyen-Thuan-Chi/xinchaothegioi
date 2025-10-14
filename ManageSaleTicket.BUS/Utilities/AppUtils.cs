using System;
using System.Linq;
using System.Text.RegularExpressions;
using ManageSaleTicket.BUS.Constants;

namespace ManageSaleTicket.BUS.Utilities
{
    /// <summary>
    /// L?p ti?n ?ch ch?a c?c ph??ng th?c h? tr?
    /// </summary>
    public static class AppUtils
    {
        /// <summary>
        /// Ki?m tra s? ?i?n tho?i c? h?p l? kh?ng
        /// </summary>
        /// <param name="phone">S? ?i?n tho?i c?n ki?m tra</param>
        /// <returns>True n?u h?p l?</returns>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            
            // Lo?i b? kho?ng tr?ng v? d?u g?ch ngang
            phone = phone.Replace(" ", "").Replace("-", "");
            
            // Ki?m tra ?? d?i v? pattern
            return phone.Length >= AppConstants.MIN_PHONE_LENGTH && 
                   phone.Length <= AppConstants.MAX_PHONE_LENGTH &&
                   Regex.IsMatch(phone, AppConstants.PHONE_PATTERN);
        }
        
        /// <summary>
        /// Ki?m tra danh s?ch gh? c? h?p l? kh?ng
        /// </summary>
        /// <param name="seats">Chu?i danh s?ch gh? (VD: "1, 2, 5")</param>
        /// <returns>True n?u h?p l?</returns>
        public static bool IsValidSeatList(string seats)
        {
            if (string.IsNullOrWhiteSpace(seats)) return false;
            
            var seatArray = seats.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var seat in seatArray)
            {
                string seatNum = seat.Trim();
                if (!int.TryParse(seatNum, out int num) || num < 1 || num > AppConstants.TOTAL_SEATS)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// T?nh t?ng ti?n t? danh s?ch gh?
        /// </summary>
        /// <param name="seatList">Chu?i danh s?ch gh?</param>
        /// <returns>T?ng ti?n</returns>
        public static int CalculateTotalAmount(string seatList)
        {
            if (string.IsNullOrEmpty(seatList))
                return 0;

            var seats = seatList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int total = 0;
            
            foreach (var seat in seats)
            {
                if (int.TryParse(seat.Trim(), out int seatNumber))
                {
                    total += AppConstants.GetSeatPrice(seatNumber);
                }
            }
            
            return total;
        }
        
        /// <summary>
        /// ??m s? l??ng v? t? danh s?ch gh?
        /// </summary>
        /// <param name="seatList">Chu?i danh s?ch gh?</param>
        /// <returns>S? l??ng v?</returns>
        public static int CountTickets(string seatList)
        {
            if (string.IsNullOrEmpty(seatList))
                return 0;

            var seats = seatList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return seats.Length;
        }
        
        /// <summary>
        /// Format ti?n t? Vi?t Nam
        /// </summary>
        /// <param name="amount">S? ti?n</param>
        /// <returns>Chu?i ??nh d?ng ti?n t?</returns>
        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0") + " VND";
        }
        
        /// <summary>
        /// Parse ti?n t? t? chu?i c? ??nh d?ng
        /// </summary>
        /// <param name="currencyString">Chu?i ti?n t? (VD: "100,000 VND")</param>
        /// <returns>S? ti?n</returns>
        public static decimal ParseCurrency(string currencyString)
        {
            if (string.IsNullOrEmpty(currencyString)) return 0;
            
            string cleanAmount = currencyString.Replace(" VND", "").Replace(",", "");
            decimal.TryParse(cleanAmount, out decimal amount);
            return amount;
        }
        
        /// <summary>
        /// S?p x?p danh s?ch gh? theo th? t? s?
        /// </summary>
        /// <param name="seatList">Chu?i danh s?ch gh?</param>
        /// <returns>Chu?i danh s?ch gh? ?? s?p x?p</returns>
        public static string SortSeatList(string seatList)
        {
            if (string.IsNullOrEmpty(seatList)) return string.Empty;
            
            var seats = seatList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(s => s.Trim())
                               .Where(s => int.TryParse(s, out _))
                               .Select(s => int.Parse(s))
                               .OrderBy(s => s)
                               .Select(s => s.ToString());
                               
            return string.Join(", ", seats);
        }
        
        /// <summary>
        /// Validate form tr??c khi submit
        /// </summary>
        /// <param name="name">T?n kh?ch h?ng</param>
        /// <param name="phone">S? ?i?n tho?i</param>
        /// <param name="region">Khu v?c</param>
        /// <param name="genderSelected">?? ch?n gi?i t?nh</param>
        /// <param name="seats">Danh s?ch gh?</param>
        /// <returns>Chu?i l?i (r?ng n?u kh?ng c? l?i)</returns>
        public static string ValidateBookingForm(string name, string phone, string region, bool genderSelected, string seats)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Vui l?ng nh?p t?n kh?ch h?ng.";
                
            if (string.IsNullOrWhiteSpace(phone))
                return "Vui l?ng nh?p s? ?i?n tho?i.";
                
            if (!IsValidPhoneNumber(phone))
                return "S? ?i?n tho?i kh?ng h?p l?! Vui l?ng nh?p s? ?i?n tho?i 10-11 ch? s? b?t ??u b?ng 0.";
                
            if (string.IsNullOrWhiteSpace(region))
                return "Vui l?ng ch?n khu v?c.";
                
            if (!genderSelected)
                return "Vui l?ng ch?n gi?i t?nh.";
                
            if (string.IsNullOrWhiteSpace(seats))
                return "Vui l?ng ch?n gh? ng?i.";
                
            if (!IsValidSeatList(seats))
                return "Danh s?ch gh? kh?ng h?p l?!";
                
            return string.Empty; // Kh?ng c? l?i
        }
    }
}