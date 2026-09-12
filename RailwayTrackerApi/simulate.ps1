# Step 1: Login and get token
$loginBody = '{"username": "admin", "password": "admin123"}'
$loginResponse = Invoke-RestMethod -Uri "http://localhost:5285/api/auth/login" -Method POST -ContentType "application/json" -Body $loginBody
$token = $loginResponse.token
Write-Host "Got token: $token"

# Step 2: Move train along route
$positions = @(
    @{lat=9.02; lon=38.75},
    @{lat=9.10; lon=39.00},
    @{lat=9.20; lon=39.25},
    @{lat=9.30; lon=39.50},
    @{lat=9.40; lon=39.75},
    @{lat=9.50; lon=40.00},
    @{lat=9.55; lon=40.25},
    @{lat=9.58; lon=40.50},
    @{lat=9.60; lon=40.75},
    @{lat=9.60; lon=41.86}
)

$headers = @{ Authorization = "Bearer $token" }

foreach ($pos in $positions) {
    $body = "{`"latitude`": $($pos.lat), `"longitude`": $($pos.lon)}"
    Invoke-RestMethod -Uri "http://localhost:5285/api/trains/1/position" -Method PUT -ContentType "application/json" -Headers $headers -Body $body
    Write-Host "Moved to: $($pos.lat), $($pos.lon)"
    Start-Sleep -Seconds 2
}