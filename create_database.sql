-- Script ?? t?o database v? b?ng Users
-- Ch?y script n?y tr?n SQL Server Management Studio

-- T?o database n?u ch?a t?n t?i
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'XinChaoTheGioiDB')
BEGIN
    CREATE DATABASE XinChaoTheGioiDB
END
GO

-- S? d?ng database
USE XinChaoTheGioiDB
GO

-- T?o b?ng Users n?u ch?a t?n t?i
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id int IDENTITY(1,1) PRIMARY KEY,
        Username nvarchar(50) NOT NULL UNIQUE,
        Password nvarchar(255) NOT NULL,
        Role nvarchar(20) NOT NULL DEFAULT 'User',
        CreatedDate datetime NOT NULL DEFAULT GETDATE(),
        IsActive bit NOT NULL DEFAULT 1
    )
    
    -- T?o index cho Username
    CREATE UNIQUE INDEX IX_Username ON Users(Username)
END
GO

-- Th?m d? li?u m?u (admin user)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    -- Password l? "admin123" ?? ???c hash
    INSERT INTO Users (Username, Password, Role, CreatedDate, IsActive)
    VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Admin', GETDATE(), 1)
END
GO

-- Hi?n th? k?t qu?
SELECT 'Database v? b?ng Users ?? ???c t?o th?nh c?ng!' AS Message
SELECT * FROM Users