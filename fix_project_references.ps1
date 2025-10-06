$projectPath = "xinchaothegioi\xinchaothegioi.csproj"
$lines = Get-Content $projectPath

# Find and fix the malformed line
for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match 'System.*Configuration') {
        # Replace the malformed line with proper formatting
        $lines[$i] = '    <Reference Include="System" />'
        # Insert the System.Configuration reference on the next line
        $lines = $lines[0..$i] + '    <Reference Include="System.Configuration" />' + $lines[($i+1)..($lines.Count-1)]
        break
    }
}

# Write back to file
$lines | Set-Content $projectPath
Write-Host "Project file fixed successfully"