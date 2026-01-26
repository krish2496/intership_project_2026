# Create Admin User Script
$adminUsername = "adminuser"
$adminEmail = "adminuser@tracker.com"
$adminPassword = "Admin123!"

Write-Host "Creating admin user..." -ForegroundColor Cyan

$body = @{
    username = $adminUsername
    email = $adminEmail
    password = $adminPassword
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri 'http://localhost:5000/api/auth/register' -Method Post -Body $body -ContentType 'application/json'
    Write-Host "Admin user registered successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Admin Credentials:" -ForegroundColor Yellow
    Write-Host "Email: $adminEmail" -ForegroundColor White
    Write-Host "Password: $adminPassword" -ForegroundColor White
    Write-Host ""
    Write-Host "Now updating user role to Admin in database..." -ForegroundColor Cyan
    
    # Update the user role to Admin in the database
    $env:PGPASSWORD = 'postgres'
    & "C:\Program Files\PostgreSQL\17\bin\psql.exe" -h localhost -p 5432 -U postgres -d TrackerDb -c "UPDATE public.\"Users\" SET \"Role\" = 'Admin' WHERE \"Email\" = '$adminEmail';"
    
    Write-Host "User role updated to Admin!" -ForegroundColor Green
    Write-Host ""
    Write-Host "You can now login with these credentials to access the admin dashboard." -ForegroundColor Green
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response: $responseBody" -ForegroundColor Red
    }
}
