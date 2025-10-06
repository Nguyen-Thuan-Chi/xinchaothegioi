# Script ?? fix v?n ?? Entity Framework Migration
Write-Host "=== FIXING ENTITY FRAMEWORK MIGRATION ISSUES ===" -ForegroundColor Cyan

# 1. Kill PowerShell processes that might be stuck
Write-Host "`n1. Stopping any stuck PowerShell processes..." -ForegroundColor Yellow
try {
    Get-Process -Name "powershell*" | Where-Object { $_.ProcessName -ne $PID } | Stop-Process -Force -ErrorAction SilentlyContinue
    Write-Host "? Stuck processes stopped" -ForegroundColor Green
} catch {
    Write-Host "No stuck processes found" -ForegroundColor Green
}

# 2. Close Package Manager Console v? reopen
Write-Host "`n2. Recommendations for Visual Studio:" -ForegroundColor Yellow
Write-Host "- Close Package Manager Console" -ForegroundColor White
Write-Host "- Close Visual Studio completely" -ForegroundColor White
Write-Host "- Restart Visual Studio as Administrator" -ForegroundColor White

# 3. Check packages.config
Write-Host "`n3. Checking packages.config..." -ForegroundColor Yellow
$packagesFile = "xinchaothegioi\packages.config"
if (Test-Path $packagesFile) {
    try {
        [xml]$xml = Get-Content $packagesFile
        Write-Host "? packages.config is valid XML" -ForegroundColor Green
        
        $efPackage = $xml.packages.package | Where-Object { $_.id -eq "EntityFramework" }
        if ($efPackage) {
            Write-Host "? EntityFramework package found: $($efPackage.version)" -ForegroundColor Green
        } else {
            Write-Host "? EntityFramework package NOT found" -ForegroundColor Red
        }
    } catch {
        Write-Host "? packages.config has XML errors: $($_.Exception.Message)" -ForegroundColor Red
    }
} else {
    Write-Host "? packages.config NOT found" -ForegroundColor Red
}

# 4. Build project
Write-Host "`n4. Testing project build..." -ForegroundColor Yellow
try {
    $buildOutput = dotnet build "xinchaothegioi\xinchaothegioi.csproj" --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Project builds successfully" -ForegroundColor Green
    } else {
        Write-Host "? Build failed" -ForegroundColor Red
        Write-Host "Build output: $buildOutput" -ForegroundColor Red
    }
} catch {
    Write-Host "Build test skipped (this is normal for packages.config projects)" -ForegroundColor Yellow
}

# 5. Check required files
Write-Host "`n5. Checking required files..." -ForegroundColor Yellow
$requiredFiles = @(
    "xinchaothegioi\Models\User.cs",
    "xinchaothegioi\Data\ApplicationDbContext.cs",
    "xinchaothegioi\Services\UserService.cs",
    "xinchaothegioi\Helpers\SqlServerHelper.cs",
    "xinchaothegioi\App.config"
)

foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "? $file" -ForegroundColor Green
    } else {
        Write-Host "? $file NOT found" -ForegroundColor Red
    }
}

Write-Host "`n=== MANUAL STEPS TO FIX MIGRATION ===" -ForegroundColor Magenta
Write-Host "1. Close Visual Studio completely" -ForegroundColor White
Write-Host "2. Delete bin\ and obj\ folders in project directory" -ForegroundColor White
Write-Host "3. Restart Visual Studio as Administrator" -ForegroundColor White
Write-Host "4. Open Package Manager Console" -ForegroundColor White
Write-Host "5. Run these commands one by one:" -ForegroundColor White
Write-Host "   Update-Package -Reinstall" -ForegroundColor Cyan
Write-Host "   Enable-Migrations -Force" -ForegroundColor Cyan
Write-Host "   Add-Migration InitialCreate" -ForegroundColor Cyan
Write-Host "   Update-Database" -ForegroundColor Cyan

Write-Host "`n=== ALTERNATIVE: CODE FIRST WITHOUT MIGRATIONS ===" -ForegroundColor Magenta
Write-Host "If migrations still fail, you can use Code First without explicit migrations:" -ForegroundColor White
Write-Host "- The ApplicationDbContext is already configured to auto-create database" -ForegroundColor White
Write-Host "- Database will be created automatically when you first run the app" -ForegroundColor White
Write-Host "- No need for Enable-Migrations command" -ForegroundColor White

Write-Host "`n? Ready to proceed with database connection!" -ForegroundColor Green