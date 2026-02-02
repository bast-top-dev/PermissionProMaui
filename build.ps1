# PermissionPro MAUI Build Script
# This script builds the MAUI project for all target platforms

param(
    [string]$Configuration = "Debug",
    [string]$Platform = "All"
)

Write-Host "🚀 Building PermissionPro MAUI Project..." -ForegroundColor Green
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow
Write-Host "Platform: $Platform" -ForegroundColor Yellow

# Function to build for a specific platform
function Build-Platform {
    param(
        [string]$TargetFramework
    )
    
    Write-Host "📱 Building for $TargetFramework..." -ForegroundColor Cyan
    
    try {
        $result = dotnet build --configuration $Configuration --framework $TargetFramework --verbosity minimal
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Successfully built for $TargetFramework" -ForegroundColor Green
        } else {
            Write-Host "❌ Failed to build for $TargetFramework" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "❌ Error building for $TargetFramework : $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
    
    return $true
}

# Function to restore packages
function Restore-Packages {
    Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Cyan
    
    try {
        dotnet restore
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Packages restored successfully" -ForegroundColor Green
            return $true
        } else {
            Write-Host "❌ Failed to restore packages" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "❌ Error restoring packages: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Main build process
try {
    # Change to project directory
    Set-Location $PSScriptRoot
    
    # Restore packages first
    if (-not (Restore-Packages)) {
        exit 1
    }
    
    # Build based on platform parameter
    switch ($Platform.ToLower()) {
        "android" {
            Build-Platform "net8.0-android"
        }
        "ios" {
            Build-Platform "net8.0-ios"
        }
        "maccatalyst" {
            Build-Platform "net8.0-maccatalyst"
        }
        "windows" {
            Build-Platform "net8.0-windows10.0.19041.0"
        }
        "all" {
            $success = $true
            
            # Build for all platforms
            $platforms = @(
                "net8.0-android",
                "net8.0-ios", 
                "net8.0-maccatalyst",
                "net8.0-windows10.0.19041.0"
            )
            
            foreach ($platform in $platforms) {
                if (-not (Build-Platform $platform)) {
                    $success = $false
                }
            }
            
            if ($success) {
                Write-Host "🎉 All platforms built successfully!" -ForegroundColor Green
            } else {
                Write-Host "⚠️ Some platforms failed to build" -ForegroundColor Yellow
                exit 1
            }
        }
        default {
            Write-Host "❌ Invalid platform specified. Use: android, ios, maccatalyst, windows, or all" -ForegroundColor Red
            exit 1
        }
    }
}
catch {
    Write-Host "❌ Build script error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
finally {
    Write-Host "🏁 Build process completed" -ForegroundColor Green
} 