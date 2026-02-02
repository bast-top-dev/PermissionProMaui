# Simple Build Test Script
Write-Host "Testing PermissionPro MAUI Build..." -ForegroundColor Green

try {
    # Test restore
    Write-Host "Restoring packages..." -ForegroundColor Yellow
    dotnet restore
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Package restore successful" -ForegroundColor Green
    } else {
        Write-Host "❌ Package restore failed" -ForegroundColor Red
        exit 1
    }
    
    # Test build for Windows (most likely to work)
    Write-Host "Building for Windows..." -ForegroundColor Yellow
    dotnet build --framework net8.0-windows10.0.19041.0 --verbosity minimal
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Windows build successful" -ForegroundColor Green
    } else {
        Write-Host "❌ Windows build failed" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "🎉 Build test completed successfully!" -ForegroundColor Green
    
} catch {
    Write-Host "❌ Build test failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} 