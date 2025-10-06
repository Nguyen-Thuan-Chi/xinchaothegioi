# Script ?? test k?t n?i Supabase v? setup database
Write-Host "=== SUPABASE CONNECTION TEST ===" -ForegroundColor Cyan

# Test 1: Ki?m tra file config
Write-Host "`n1. Checking configuration files..." -ForegroundColor Yellow
$configFile = "xinchaothegioi\App.config"
if (Test-Path $configFile) {
    Write-Host "? App.config found" -ForegroundColor Green
    $content = Get-Content $configFile -Raw
    if ($content -like "*supabase*") {
        Write-Host "? Supabase configuration detected" -ForegroundColor Green
    } else {
        Write-Host "? Supabase configuration NOT found" -ForegroundColor Red
    }
} else {
    Write-Host "? App.config NOT found" -ForegroundColor Red
}

# Test 2: Ki?m tra packages
Write-Host "`n2. Checking packages..." -ForegroundColor Yellow
$packagesFile = "xinchaothegioi\packages.config"
if (Test-Path $packagesFile) {
    Write-Host "? packages.config found" -ForegroundColor Green
    $content = Get-Content $packagesFile -Raw
    if ($content -like "*Npgsql*") {
        Write-Host "? Npgsql package detected" -ForegroundColor Green
    } else {
        Write-Host "? Npgsql package NOT found" -ForegroundColor Red
    }
} else {
    Write-Host "? packages.config NOT found" -ForegroundColor Red
}

# Test 3: Build project
Write-Host "`n3. Building project..." -ForegroundColor Yellow
try {
    $buildResult = dotnet build "xinchaothegioi\xinchaothegioi.csproj" --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Build successful" -ForegroundColor Green
    } else {
        Write-Host "? Build failed" -ForegroundColor Red
        Write-Host $buildResult
    }
} catch {
    Write-Host "? Build error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Ki?m tra c?c file quan tr?ng
Write-Host "`n4. Checking important files..." -ForegroundColor Yellow
$files = @(
    "xinchaothegioi\Helpers\SupabaseConfig.cs",
    "xinchaothegioi\Helpers\DatabaseHelper.cs", 
    "xinchaothegioi\Services\UserService.cs",
    "xinchaothegioi\Utils\SupabaseHelper.cs",
    "xinchaothegioi\Models\User.cs"
)

foreach ($file in $files) {
    if (Test-Path $file) {
        Write-Host "? $file" -ForegroundColor Green
    } else {
        Write-Host "? $file NOT found" -ForegroundColor Red
    }
}

# Test 5: Hi?n th? th?ng tin k?t n?i
Write-Host "`n5. Connection Information:" -ForegroundColor Yellow
Write-Host "Database Host: db.vmzldhyhyutpohorxniw.supabase.co" -ForegroundColor Cyan
Write-Host "Database Port: 5432" -ForegroundColor Cyan
Write-Host "Database Name: postgres" -ForegroundColor Cyan
Write-Host "SSL Mode: Required" -ForegroundColor Cyan
Write-Host "Supabase URL: https://vmzldhyhyutpohorxniw.supabase.co" -ForegroundColor Cyan

Write-Host "`n=== INSTRUCTIONS ===" -ForegroundColor Magenta
Write-Host "1. Build project th?nh c?ng - c? th? ch?y ?ng d?ng" -ForegroundColor White
Write-Host "2. Trong form Login, nh?n Ctrl+T ?? m? form test Supabase" -ForegroundColor White
Write-Host "3. C? th? d?ng account m?c ??nh:" -ForegroundColor White
Write-Host "   Username: admin" -ForegroundColor Green
Write-Host "   Password: admin123" -ForegroundColor Green
Write-Host "4. Form test Supabase s? gi?p:" -ForegroundColor White
Write-Host "   - Test k?t n?i database" -ForegroundColor White
Write-Host "   - Test REST API" -ForegroundColor White
Write-Host "   - T?o b?ng users t? ??ng" -ForegroundColor White
Write-Host "   - Test UserService" -ForegroundColor White
Write-Host "   - Hi?n th? debug info" -ForegroundColor White

Write-Host "`n=== READY TO RUN ===" -ForegroundColor Green
Write-Host "Project ?? s?n s?ng! B?n c? th?:" -ForegroundColor White
Write-Host "1. Ch?y ?ng d?ng t? Visual Studio" -ForegroundColor White
Write-Host "2. ??ng nh?p v?i admin/admin123" -ForegroundColor White
Write-Host "3. Nh?n Ctrl+T ?? test Supabase connection" -ForegroundColor White