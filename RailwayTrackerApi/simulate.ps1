# Train 1 route: Addis Ababa -> Dire Dawa (step by step)
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

foreach ($pos in $positions) {
    $body = "{`"latitude`": $($pos.lat), `"longitude`": $($pos.lon)}"
    Invoke-RestMethod -Uri "http://localhost:5285/api/trains/1/position" -Method PUT -ContentType "application/json" -Body $body
    Write-Host "Moved to: $($pos.lat), $($pos.lon)"
    Start-Sleep -Seconds 2
}