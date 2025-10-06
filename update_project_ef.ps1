# Script to add Entity Framework references to the project
$projectPath = "xinchaothegioi\xinchaothegioi.csproj"

# Read project content
[xml]$project = Get-Content $projectPath

# Find the ItemGroup that contains references
$referenceItemGroup = $project.Project.ItemGroup | Where-Object { $_.Reference -ne $null } | Select-Object -First 1

if ($referenceItemGroup -eq $null) {
    # Create new ItemGroup for references if it doesn't exist
    $referenceItemGroup = $project.CreateElement("ItemGroup")
    $project.Project.AppendChild($referenceItemGroup)
}

# Add Entity Framework reference
$efRef = $project.CreateElement("Reference")
$efRef.SetAttribute("Include", "EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL")
$efHintPath = $project.CreateElement("HintPath")
$efHintPath.InnerText = "packages\EntityFramework.6.4.4\lib\net45\EntityFramework.dll"
$efRef.AppendChild($efHintPath)
$referenceItemGroup.AppendChild($efRef)

# Add Entity Framework SqlServer reference
$efSqlRef = $project.CreateElement("Reference")
$efSqlRef.SetAttribute("Include", "EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL")
$efSqlHintPath = $project.CreateElement("HintPath")
$efSqlHintPath.InnerText = "packages\EntityFramework.6.4.4\lib\net45\EntityFramework.SqlServer.dll"
$efSqlRef.AppendChild($efSqlHintPath)
$referenceItemGroup.AppendChild($efSqlRef)

# Add System.Data.SqlClient reference
$sqlClientRef = $project.CreateElement("Reference")
$sqlClientRef.SetAttribute("Include", "System.Data.SqlClient, Version=4.6.1.5, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL")
$sqlClientHintPath = $project.CreateElement("HintPath")
$sqlClientHintPath.InnerText = "packages\System.Data.SqlClient.4.8.5\lib\net461\System.Data.SqlClient.dll"
$sqlClientRef.AppendChild($sqlClientHintPath)
$referenceItemGroup.AppendChild($sqlClientRef)

# Add System.ComponentModel.DataAnnotations if not exists
$dataAnnotationsExists = $referenceItemGroup.Reference | Where-Object { $_.Include -like "*System.ComponentModel.DataAnnotations*" }
if (-not $dataAnnotationsExists) {
    $dataAnnotationsRef = $project.CreateElement("Reference")
    $dataAnnotationsRef.SetAttribute("Include", "System.ComponentModel.DataAnnotations")
    $referenceItemGroup.AppendChild($dataAnnotationsRef)
}

# Save the project file
$project.Save($projectPath)
Write-Host "Entity Framework references added successfully"