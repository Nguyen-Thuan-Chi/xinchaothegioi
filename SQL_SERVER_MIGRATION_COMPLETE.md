# SQL SERVER CONNECTION SETUP - COMPLETE

## ?? Tr?ng th?i hi?n t?i
? **?? ho?n th?nh migration t? Supabase sang SQL Server!**
- Build project th?nh c?ng
- T?t c? l?i ?? ???c s?a
- Entity Framework 6.4.4 ?? ???c c?u h?nh
- SQL Server connection ?? s?n s?ng

## ?? C?u h?nh SQL Server

### Connection Information
```
Server Name: LAPTOP-PN16MELH
Database: XinChaoTheGioiDB  
Authentication: Windows Authentication
Provider: System.Data.SqlClient
```

### Connection String
```
Data Source=LAPTOP-PN16MELH;Initial Catalog=XinChaoTheGioiDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
```

## ?? Files ?? ???c t?o/c?p nh?t

### Core Files
- ? `Models/User.cs` - Entity model cho b?ng Users
- ? `Data/ApplicationDbContext.cs` - Entity Framework DbContext  
- ? `Services/UserService.cs` - Service layer cho user operations
- ? `Helpers/SqlServerHelper.cs` - Helper cho SQL Server operations
- ? `App.config` - Database connection configuration

### Test Files  
- ? `frmSupabaseTest.cs` - Form test SQL Server connection
- ? `test_sql_server_connection.ps1` - PowerShell script ki?m tra

### Updated Files
- ? `frmRegister.cs` - Updated ?? s? d?ng SQL Server
- ? `packages.config` - Entity Framework packages

## ??? Database Schema

### B?ng Users
```sql
CREATE TABLE Users (
    UserId int IDENTITY(1,1) PRIMARY KEY,
    Username nvarchar(50) NOT NULL UNIQUE,
    Password nvarchar(255) NOT NULL,
    Role nvarchar(20) NOT NULL DEFAULT 'User',
    CreatedDate datetime2 NOT NULL DEFAULT GETDATE(),
    IsActive bit NOT NULL DEFAULT 1
)
```

## ?? C?ch s? d?ng

### 1. Ki?m tra SQL Server
Ch?y script PowerShell ?? ki?m tra:
```powershell
.\test_sql_server_connection.ps1
```

### 2. Ch?y ?ng d?ng
- Build v? run t? Visual Studio
- Form Register c? n?t "Test Connection" ?? ki?m tra
- Form "SQL Server Test" ?? test chi ti?t

### 3. Test c?c ch?c n?ng
- **??ng k? user m?i**: S? d?ng form Register
- **Test connection**: Click "Test Connection" 
- **Admin account**: admin/admin123 (t? ??ng t?o)

## ?? Troubleshooting

### N?u k?t n?i th?t b?i:

1. **Ki?m tra SQL Server Service**
   ```
   - SQL Server (MSSQLSERVER) - Running
   - SQL Server Browser - Running  
   ```

2. **Enable TCP/IP Protocol**
   - M? SQL Server Configuration Manager
   - Enable TCP/IP trong SQL Server Network Configuration
   - Restart SQL Server service

3. **Ki?m tra Windows Authentication**
   - ??m b?o Windows user c? quy?n truy c?p SQL Server
   - Test b?ng SQL Server Management Studio

4. **Alternative: LocalDB**
   N?u v?n l?i, ??i connection string th?nh:
   ```
   Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=XinChaoTheGioiDB;Integrated Security=True
   ```

## ??? API Methods

### UserService
- `TestConnection()` - Test database connection
- `RegisterUser(username, password)` - ??ng k? user m?i  
- `ValidateUser(username, password)` - X?c th?c login
- `GetUser(username)` - L?y th?ng tin user
- `InitializeDatabase()` - T?o database v? admin user

### SqlServerHelper
- `TestConnection()` - Test SQL Server connection
- `CreateDatabaseIfNotExists()` - T?o database t? ??ng
- `HashPassword(password)` - Hash password
- `VerifyPassword(password, hash)` - Verify password

## ? Features

### Automatic Database Creation
- Database s? ???c t?o t? ??ng khi ch?y app l?n ??u
- B?ng Users ???c t?o b?i Entity Framework Code First
- Admin user (admin/admin123) ???c t?o t? ??ng

### Security
- Password ???c hash b?ng SHA256 + salt
- Windows Authentication ?? b?o m?t
- Connection string secure

### Error Handling
- Comprehensive error handling trong t?t c? operations
- Detailed error messages cho debugging
- Connection timeout v? retry logic

## ?? K?t lu?n

**Ho?n t?t migration t? Supabase sang SQL Server!**

B?y gi? b?n c? th?:
- ? K?t n?i SQL Server v?i Windows Authentication
- ? Qu?n l? users v?i Entity Framework  
- ? Test connection v? debug d? d?ng
- ? ??ng k? v? ??ng nh?p users
- ? Database t? ??ng setup

**S?n s?ng s? d?ng!** ??