# Script to enable Entity Framework migrations
# Run this in Package Manager Console

# Enable migrations
Enable-Migrations -ContextTypeName xinchaothegioi.Data.ApplicationDbContext -Force

# Add initial migration
Add-Migration InitialCreate

# Update database to latest migration
Update-Database -Verbose