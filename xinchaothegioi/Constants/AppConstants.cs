using System;

namespace xinchaothegioi
{
    /// <summary>
    /// L?p ch?a c?c h?ng s? v? c?u h?nh c?a ?ng d?ng
    /// </summary>
    public static class AppConstants
    {
        // Th?ng tin ??ng nh?p
        public const string DEFAULT_USERNAME = "Admin";
        public const string DEFAULT_PASSWORD = "Admin123";
        
        // C?u h?nh gh? ng?i
        public const int TOTAL_SEATS = 24;
        public const int SEATS_PER_ROW = 4;
        public const int TOTAL_ROWS = 6;
        
        // Gi? v? theo v? tr?
        public const int PRICE_COLUMN_1_4_ROW_1_2 = 80000;  // C?t 1,4 - H?ng 1,2
        public const int PRICE_COLUMN_1_4_ROW_3_4 = 90000;  // C?t 1,4 - H?ng 3,4
        public const int PRICE_COLUMN_2_3_ROW_1_2 = 100000; // C?t 2,3 - H?ng 1,2
        public const int PRICE_COLUMN_2_3_ROW_3_4 = 110000; // C?t 2,3 - H?ng 3,4
        
        // M?u s?c gh?
        public static readonly System.Drawing.Color SEAT_AVAILABLE = System.Drawing.SystemColors.Control;
        public static readonly System.Drawing.Color SEAT_SELECTED = System.Drawing.Color.Yellow;
        public static readonly System.Drawing.Color SEAT_OCCUPIED = System.Drawing.Color.Red;
        
        // Format ng?y th?ng
        public const string DATE_FORMAT = "dd/MM/yyyy";
        public const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        
        // Format m? h?a ??n
        public const string INVOICE_ID_PREFIX = "HD";
        public const string INVOICE_ID_FORMAT = "{0}{1:yyyyMMdd}{2:D3}";
        
        // Th?ng b?o
        public const string MSG_LOGIN_SUCCESS = "??ng nh?p th?nh c?ng!";
        public const string MSG_LOGIN_FAILED = "Th?ng tin ??ng nh?p kh?ng ch?nh x?c!";
        public const string MSG_MISSING_INFO = "Vui l?ng nh?p ??y ?? th?ng tin!";
        public const string MSG_SEAT_OCCUPIED = "Gh? n?y ?? ???c mua!";
        public const string MSG_BOOKING_SUCCESS = "??t v? th?nh c?ng!";
        public const string MSG_DELETE_CONFIRM = "B?n c? ch?c ch?n mu?n x?a?";
        
        // Validation
        public const int MIN_PHONE_LENGTH = 10;
        public const int MAX_PHONE_LENGTH = 11;
        public const string PHONE_PATTERN = @"^0\d{9,10}$";
        
        // Export file
        public const string EXPORT_CSV_FILTER = "CSV Files|*.csv";
        public const string EXPORT_IMAGE_FILTER = "PNG Files|*.png";
        public const string EXPORT_EXCEL_FILTER = "Excel Files|*.xlsx";
        
        /// <summary>
        /// L?y gi? v? theo s? gh? (1-24)
        /// Layout gh? 6 h?ng x 4 c?t:
        /// H?ng 1: 1, 2, 3, 4
        /// H?ng 2: 5, 6, 7, 8  
        /// H?ng 3: 9,10,11,12
        /// H?ng 4: 13,14,15,16
        /// H?ng 5: 17,18,19,20
        /// H?ng 6: 21,22,23,24
        /// </summary>
        /// <param name="seatNumber">S? gh? (1-24)</param>
        /// <returns>Gi? v?</returns>
        public static int GetSeatPrice(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > TOTAL_SEATS)
                return PRICE_COLUMN_1_4_ROW_1_2; // M?c ??nh
            
            // X?c ??nh h?ng v? c?t c?a gh?
            int row = ((seatNumber - 1) / SEATS_PER_ROW) + 1; // H?ng 1-6
            int col = ((seatNumber - 1) % SEATS_PER_ROW) + 1; // C?t 1-4
            
            // C?t 1 v? 4 (c?t ??u v? cu?i)
            bool isColumn1Or4 = (col == 1 || col == 4);
            // C?t 2 v? 3 (c?t gi?a)  
            bool isColumn2Or3 = (col == 2 || col == 3);
            // H?ng 3 v? 4 (h?ng gi?a t?t nh?t)
            bool isRow3Or4 = (row == 3 || row == 4);
            
            if (isColumn1Or4)
            {
                // C?t 1 v? 4
                if (isRow3Or4)
                    return PRICE_COLUMN_1_4_ROW_3_4; // H?ng 3,4: 90k
                else
                    return PRICE_COLUMN_1_4_ROW_1_2; // H?ng kh?c: 80k
            }
            else if (isColumn2Or3)
            {
                // C?t 2 v? 3
                if (isRow3Or4)
                    return PRICE_COLUMN_2_3_ROW_3_4; // H?ng 3,4: 110k
                else
                    return PRICE_COLUMN_2_3_ROW_1_2; // H?ng kh?c: 100k
            }

            return PRICE_COLUMN_1_4_ROW_1_2; // M?c ??nh
        }
        
        /// <summary>
        /// L?y th?ng tin v? tr? gh? (h?ng, c?t)
        /// </summary>
        /// <param name="seatNumber">S? gh? (1-24)</param>
        /// <returns>Tuple(row, col)</returns>
        public static (int row, int col) GetSeatPosition(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > TOTAL_SEATS)
                return (1, 1);
                
            int row = ((seatNumber - 1) / SEATS_PER_ROW) + 1;
            int col = ((seatNumber - 1) % SEATS_PER_ROW) + 1;
            return (row, col);
        }
        
        /// <summary>
        /// L?y m? t? v? tr? gh?
        /// </summary>
        /// <param name="seatNumber">S? gh? (1-24)</param>
        /// <returns>M? t? v? tr?</returns>
        public static string GetSeatDescription(int seatNumber)
        {
            var (row, col) = GetSeatPosition(seatNumber);
            int price = GetSeatPrice(seatNumber);
            
            string position;
            if (col == 1 || col == 4)
                position = "C?nh l?i ?i";
            else
                position = "Gi?a";
                
            return $"H?ng {row}, {position} - {price:N0} VND";
        }
        
        /// <summary>
        /// T?o m? h?a ??n
        /// </summary>
        /// <param name="orderInDay">S? th? t? trong ng?y</param>
        /// <returns>M? h?a ??n</returns>
        public static string GenerateInvoiceId(int orderInDay)
        {
            return string.Format(INVOICE_ID_FORMAT, INVOICE_ID_PREFIX, DateTime.Now, orderInDay);
        }
    }
}