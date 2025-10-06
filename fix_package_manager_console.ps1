# Script ?? fix Package Manager Console b? ??ng
Write-Host "=== FIXING PACKAGE MANAGER CONSOLE ISSUES ===" -ForegroundColor Cyan

# 1. Kill t?t c? c?c ti?n tr?nh PowerShell c? th? b? stuck
Write-Host "`n1. Stopping stuck PowerShell processes..." -ForegroundColor Yellow
try {
    # Kill PowerShell ISE
    Get-Process -Name "PowerShell_ISE" -ErrorAction SilentlyContinue | Stop-Process -Force
    # Kill PowerShell
    Get-Process -Name "powershell" -ErrorAction SilentlyContinue | Where-Object { $_.Id -ne $PID } | Stop-Process -Force
    # Kill devenv processes n?u c?n
    Get-Process -Name "devenv" -ErrorAction SilentlyContinue | Stop-Process -Force
    Write-Host "? Processes terminated" -ForegroundColor Green
} catch {
    Write-Host "! Some processes may still be running" -ForegroundColor Yellow
}

# 2. Clear NuGet cache
Write-Host "`n2. Clearing NuGet cache..." -ForegroundColor Yellow
try {
    # Clear global packages cache
    if (Get-Command "dotnet" -ErrorAction SilentlyContinue) {
        dotnet nuget locals all --clear
        Write-Host "? NuGet cache cleared" -ForegroundColor Green
    }
} catch {
    Write-Host "! Could not clear NuGet cache" -ForegroundColor Yellow
}

# 3. Clean project directories
Write-Host "`n3. Cleaning project directories..." -ForegroundColor Yellow
$projectPath = "xinchaothegioi"
if (Test-Path $projectPath) {
    # Remove bin v? obj folders
    $binPath = Join-Path $projectPath "bin"
    $objPath = Join-Path $projectPath "obj"
    
    if (Test-Path $binPath) {
        Remove-Item $binPath -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "? Removed bin folder" -ForegroundColor Green
    }
    
    if (Test-Path $objPath) {
        Remove-Item $objPath -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "? Removed obj folder" -ForegroundColor Green
    }
    
    # Remove packages folder n?u c?
    $packagesPath = "packages"
    if (Test-Path $packagesPath) {
        Remove-Item $packagesPath -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "? Removed packages folder" -ForegroundColor Green
    }
}

# 4. Backup v? recreate packages.config
Write-Host "`n4. Fixing packages.config..." -ForegroundColor Yellow
$packagesConfigPath = Join-Path $projectPath "packages.config"
if (Test-Path $packagesConfigPath) {
    # Backup original
    Copy-Item $packagesConfigPath "$packagesConfigPath.backup" -Force
    Write-Host "? Backed up packages.config" -ForegroundColor Green
    
    # Validate XML
    try {
        [xml]$xml = Get-Content $packagesConfigPath
        Write-Host "? packages.config is valid XML" -ForegroundColor Green
    } catch {
        Write-Host "? packages.config has XML errors" -ForegroundColor Red
        Write-Host "Restoring from backup..." -ForegroundColor Yellow
    }
}

Write-Host "`n=== MANUAL STEPS ===" -ForegroundColor Magenta
Write-Host "Now follow these steps manually:" -ForegroundColor White
Write-Host "1. Close Visual Studio completely" -ForegroundColor Cyan
Write-Host "2. Wait 30 seconds" -ForegroundColor Cyan
Write-Host "3. Restart Visual Studio as Administrator" -ForegroundColor Cyan
Write-Host "4. Open your project" -ForegroundColor Cyan
Write-Host "5. Let Visual Studio restore packages automatically" -ForegroundColor Cyan
Write-Host "6. If that doesn't work, right-click solution Å® Restore NuGet Packages" -ForegroundColor Cyan

Write-Host "`n=== ALTERNATIVE: SKIP MIGRATIONS ===" -ForegroundColor Magenta
Write-Host "If Package Manager Console still doesn't work:" -ForegroundColor White
Write-Host "- You can skip Entity Framework Migrations" -ForegroundColor Cyan
Write-Host "- Use Code First with automatic database creation" -ForegroundColor Cyan
Write-Host "- Database will be created when you run the app" -ForegroundColor Cyan
Write-Host "- No need for Enable-Migrations command" -ForegroundColor Cyan

Write-Host "`n=== USING PLAIN SQL INSTEAD ===" -ForegroundColor Magenta
Write-Host "Or use plain SQL Server connection without EF:" -ForegroundColor White
Write-Host "- SqlServerHelper.cs is already configured" -ForegroundColor Cyan
Write-Host "- Uses SqlConnection directly" -ForegroundColor Cyan
Write-Host "- More reliable than EF for simple scenarios" -ForegroundColor Cyan

Write-Host "`n? Clean up completed!" -ForegroundColor Green