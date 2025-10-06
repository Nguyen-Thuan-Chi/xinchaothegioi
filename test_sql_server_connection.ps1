# Script ki?m tra SQL Server v? fix c?c v?n ??
Write-Host "=== SQL SERVER CONNECTION TEST ===" -ForegroundColor Cyan

# 1. Ki?m tra SQL Server Service ?ang ch?y
Write-Host "`n1. Checking SQL Server Services..." -ForegroundColor Yellow
try {
    $sqlServices = Get-Service -Name "*sql*" | Where-Object { $_.Status -eq "Running" }
    if ($sqlServices.Count -gt 0) {
        Write-Host "? SQL Server services are running:" -ForegroundColor Green
        foreach ($service in $sqlServices) {
            Write-Host "  - $($service.Name): $($service.Status)" -ForegroundColor Green
        }
    } else {
        Write-Host "? No SQL Server services found running" -ForegroundColor Red
        Write-Host "  Try starting SQL Server services manually" -ForegroundColor Yellow
    }
} catch {
    Write-Host "? Error checking services: $($_.Exception.Message)" -ForegroundColor Red
}

# 2. Ki?m tra SQL Server instance
Write-Host "`n2. Checking SQL Server Instance..." -ForegroundColor Yellow
try {
    $sqlProcess = Get-Process -Name "sqlservr" -ErrorAction SilentlyContinue
    if ($sqlProcess) {
        Write-Host "? SQL Server process is running" -ForegroundColor Green
    } else {
        Write-Host "? SQL Server process not found" -ForegroundColor Red
    }
} catch {
    Write-Host "Warning: Could not check SQL Server process" -ForegroundColor Yellow
}

# 3. Test k?t n?i v?i sqlcmd (n?u c?)
Write-Host "`n3. Testing connection with sqlcmd..." -ForegroundColor Yellow
try {
    $result = sqlcmd -S "LAPTOP-PN16MELH" -Q "SELECT @@VERSION" -E 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? sqlcmd connection successful" -ForegroundColor Green
    } else {
        Write-Host "? sqlcmd connection failed" -ForegroundColor Red
        Write-Host "Error: $result" -ForegroundColor Red
    }
} catch {
    Write-Host "sqlcmd not available or connection failed" -ForegroundColor Yellow
}

# 4. Ki?m tra project files
Write-Host "`n4. Checking project configuration..." -ForegroundColor Yellow
$requiredFiles = @(
    "xinchaothegioi\App.config",
    "xinchaothegioi\Models\User.cs",
    "xinchaothegioi\Data\ApplicationDbContext.cs",
    "xinchaothegioi\Services\UserService.cs",
    "xinchaothegioi\Helpers\SqlServerHelper.cs"
)

foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "? $file" -ForegroundColor Green
    } else {
        Write-Host "? $file NOT found" -ForegroundColor Red
    }
}

# 5. Ki?m tra connection string
Write-Host "`n5. Checking connection string..." -ForegroundColor Yellow
$configFile = "xinchaothegioi\App.config"
if (Test-Path $configFile) {
    $content = Get-Content $configFile -Raw
    if ($content -like "*LAPTOP-PN16MELH*") {
        Write-Host "? Connection string configured for LAPTOP-PN16MELH" -ForegroundColor Green
    } else {
        Write-Host "? Connection string may not be configured correctly" -ForegroundColor Red
    }
    
    if ($content -like "*Integrated Security=True*") {
        Write-Host "? Windows Authentication enabled" -ForegroundColor Green
    } else {
        Write-Host "? Windows Authentication not configured" -ForegroundColor Red
    }
}

Write-Host "`n=== TROUBLESHOOTING GUIDE ===" -ForegroundColor Magenta
Write-Host "If connection fails, try these steps:" -ForegroundColor White
Write-Host "1. Start SQL Server services:" -ForegroundColor Yellow
Write-Host "   - SQL Server (MSSQLSERVER)" -ForegroundColor White
Write-Host "   - SQL Server Browser" -ForegroundColor White
Write-Host "   - SQL Server Agent (optional)" -ForegroundColor White

Write-Host "`n2. Enable TCP/IP protocol:" -ForegroundColor Yellow
Write-Host "   - Open SQL Server Configuration Manager" -ForegroundColor White
Write-Host "   - Go to SQL Server Network Configuration" -ForegroundColor White
Write-Host "   - Enable TCP/IP protocol" -ForegroundColor White
Write-Host "   - Restart SQL Server service" -ForegroundColor White

Write-Host "`n3. Check Windows Authentication:" -ForegroundColor Yellow
Write-Host "   - Open SQL Server Management Studio" -ForegroundColor White
Write-Host "   - Connect with Windows Authentication" -ForegroundColor White
Write-Host "   - Verify your Windows user has access" -ForegroundColor White

Write-Host "`n4. Alternative: Use SQL Server Express LocalDB:" -ForegroundColor Yellow
Write-Host "   Change connection string to:" -ForegroundColor White
Write-Host "   Data Source=(localdb)\MSSQLLocalDB;..." -ForegroundColor Cyan

Write-Host "`n=== NEXT STEPS ===" -ForegroundColor Green
Write-Host "1. Run the application from Visual Studio" -ForegroundColor White
Write-Host "2. Try to register a new user" -ForegroundColor White
Write-Host "3. Use the Test Connection button in Register form" -ForegroundColor White
Write-Host "4. Check the SQL Server Test form for detailed diagnostics" -ForegroundColor White

Write-Host "`n? All files are ready for SQL Server connection!" -ForegroundColor Green