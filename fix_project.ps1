$projectPath = "xinchaothegioi\xinchaothegioi.csproj"
$content = Get-Content $projectPath
$newContent = $content -replace '(<Reference Include="System" />)', ('$1' + [Environment]::NewLine + '    <Reference Include="System.Configuration" />')
$newContent | Set-Content $projectPath
Write-Host "System.Configuration reference added successfully"