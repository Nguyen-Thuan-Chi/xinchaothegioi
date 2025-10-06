# SUPABASE CONNECTION GUIDE

## ?? T?ng quan
D? ?n ?? ???c c?u h?nh ?? k?t n?i v?i Supabase PostgreSQL database. T?t c? c?c component c?n thi?t ?? ???c thi?t l?p v? s?n s?ng s? d?ng.

## ? Tr?ng th?i hi?n t?i
- ? Build project th?nh c?ng
- ? Supabase configuration ho?n t?t
- ? Database helpers ?? s?n s?ng
- ? User service ?? ???c t?i ?u
- ? Test tools ?? ???c t?ch h?p

## ?? C?u h?nh Supabase

### Database Connection
```
Host: db.vmzldhyhyutpohorxniw.supabase.co
Port: 5432
Database: postgres
Username: postgres
Password: Jonhejan14@gmail.com
SSL Mode: Required
```

### Supabase Project
```
URL: https://vmzldhyhyutpohorxniw.supabase.co
Anon Key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## ?? C?ch s? d?ng

### 1. Ch?y ?ng d?ng
- M? Visual Studio
- Build v? ch?y project
- Form login s? hi?n ra

### 2. Test k?t n?i Supabase
- Trong form login, nh?n **Ctrl + T**
- Form test Supabase s? m? ra
- Click c?c n?t ?? test:
  - **Test DB Connection**: Ki?m tra k?t n?i database
  - **Test REST API**: Ki?m tra Supabase REST API
  - **Create Tables**: T?o b?ng users t? ??ng
  - **Test User Service**: Test c?c ch?c n?ng user
  - **Get Debug Info**: Hi?n th? th?ng tin debug

### 3. ??ng nh?p
- Username: `admin`
- Password: `admin123`

*(T?i kho?n admin s? ???c t?o t? ??ng khi ch?y ?ng d?ng l?n ??u)*

## ?? C?u tr?c file quan tr?ng

### Core Files
- `Helpers/SupabaseConfig.cs` - C?u h?nh Supabase
- `Helpers/DatabaseHelper.cs` - Utility database
- `Services/UserService.cs` - Service qu?n l? user
- `Utils/SupabaseHelper.cs` - Helper cho Supabase
- `Models/User.cs` - Model User

### Test Files
- `frmSupabaseTest.cs` - Form test k?t n?i
- `test_supabase_connection.ps1` - Script test

### Config Files
- `App.config` - C?u h?nh ?ng d?ng
- `packages.config` - NuGet packages

## ?? Troubleshooting

### N?u kh?ng k?t n?i ???c database:
1. Ki?m tra internet connection
2. Xem App.config c? ??ng th?ng tin Supabase kh?ng
3. Ch?y form test Supabase ?? debug chi ti?t

### N?u c? l?i build:
1. Restore NuGet packages
2. Rebuild solution
3. Ki?m tra .NET Framework 4.7.2

### N?u kh?ng t?o ???c b?ng:
1. M? form test Supabase
2. Click "Create Tables"
3. Xem log ?? bi?t l?i chi ti?t

## ?? Database Schema

### B?ng users
```sql
CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(20) NOT NULL DEFAULT 'User',
    full_name VARCHAR(100),
    phone_number VARCHAR(15),
    email VARCHAR(100),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE
);
```

## ??? API Methods

### UserService Methods
- `TestConnection()` - Test k?t n?i DB
- `ValidateUser(username, password)` - X?c th?c user
- `RegisterUser(...)` - ??ng k? user m?i
- `GetUser(username)` - L?y th?ng tin user
- `GetAllUsers()` - L?y t?t c? users
- `UpdateUser(user)` - C?p nh?t user
- `DeleteUser(userId)` - X?a user (soft delete)

### SupabaseHelper Methods
- `TestSupabaseConnection()` - Test DB connection
- `TestSupabaseRestApiAsync()` - Test REST API
- `CreateUsersTableIfNotExists()` - T?o b?ng users
- `GetPostgreSQLVersion()` - L?y version PostgreSQL
- `GetAllTables()` - L?y danh s?ch tables
- `GetSupabaseDebugInfo()` - L?y debug info

## ?? K?t lu?n

H? th?ng ?? ???c thi?t l?p ho?n ch?nh v? s?n s?ng ??:
- K?t n?i v?i Supabase PostgreSQL
- Qu?n l? users
- Test v? debug k?t n?i
- M? r?ng th?m c?c features kh?c

**T?t c? ?? ho?t ??ng! B?n c? th? b?t ??u s? d?ng ngay.**