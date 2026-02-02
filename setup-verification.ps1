# Setup Verification Script for MAUI Development Environment
Write-Host "MAUI DEVELOPMENT ENVIRONMENT VERIFICATION" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Green
Write-Host ""

# Check .NET SDK
Write-Host "Checking .NET 8 SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version 2>$null
    if ($dotnetVersion -match "8\.0\.") {
        Write-Host "  PASS: .NET 8 SDK installed - Version $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "  FAIL: .NET 8 SDK not found or wrong version" -ForegroundColor Red
        Write-Host "  Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  FAIL: .NET SDK not installed" -ForegroundColor Red
    Write-Host "  Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
}

Write-Host ""

# Check MAUI workload
Write-Host "Checking MAUI workload..." -ForegroundColor Yellow
try {
    $workloads = dotnet workload list 2>$null
    if ($workloads -match "maui") {
        Write-Host "  PASS: MAUI workload installed" -ForegroundColor Green
    } else {
        Write-Host "  WARN: MAUI workload not found" -ForegroundColor Yellow
        Write-Host "  Run: dotnet workload install maui" -ForegroundColor Cyan
    }
} catch {
    Write-Host "  FAIL: Could not check MAUI workload" -ForegroundColor Red
}

Write-Host ""

# Check Visual Studio installation
Write-Host "Checking Visual Studio 2022..." -ForegroundColor Yellow
$vsPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise"
)

$vsFound = $false
foreach ($path in $vsPaths) {
    if (Test-Path $path) {
        Write-Host "  PASS: Visual Studio 2022 found at $path" -ForegroundColor Green
        $vsFound = $true
        break
    }
}

if (-not $vsFound) {
    Write-Host "  FAIL: Visual Studio 2022 not found" -ForegroundColor Red
    Write-Host "  Please install Visual Studio 2022 from: https://visualstudio.microsoft.com/downloads/" -ForegroundColor Yellow
}

Write-Host ""

# Check project structure
Write-Host "Checking MAUI project structure..." -ForegroundColor Yellow
$requiredFiles = @(
    "PermissionProMaui.csproj",
    "App.xaml",
    "App.xaml.cs",
    "MauiProgram.cs"
)

$allFilesPresent = $true
foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "  PASS: $file" -ForegroundColor Green
    } else {
        Write-Host "  FAIL: $file missing" -ForegroundColor Red
        $allFilesPresent = $false
    }
}

Write-Host ""

# Check if we can restore packages
Write-Host "Checking package restore..." -ForegroundColor Yellow
try {
    $restoreResult = dotnet restore 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  PASS: Package restore successful" -ForegroundColor Green
    } else {
        Write-Host "  FAIL: Package restore failed" -ForegroundColor Red
        Write-Host "  Error: $restoreResult" -ForegroundColor Red
    }
} catch {
    Write-Host "  FAIL: Could not restore packages" -ForegroundColor Red
}

Write-Host ""

# Check if we can build the project
Write-Host "Checking project build..." -ForegroundColor Yellow
try {
    $buildResult = dotnet build --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  PASS: Project builds successfully" -ForegroundColor Green
    } else {
        Write-Host "  FAIL: Project build failed" -ForegroundColor Red
        Write-Host "  Error: $buildResult" -ForegroundColor Red
    }
} catch {
    Write-Host "  FAIL: Could not build project" -ForegroundColor Red
}

Write-Host ""

# Summary
Write-Host "SETUP VERIFICATION SUMMARY:" -ForegroundColor Yellow
Write-Host "===========================" -ForegroundColor Yellow

$checks = @(
    @{ Name = ".NET 8 SDK"; Status = $dotnetVersion -match "8\.0\." },
    @{ Name = "MAUI Workload"; Status = $workloads -match "maui" },
    @{ Name = "Visual Studio 2022"; Status = $vsFound },
    @{ Name = "Project Structure"; Status = $allFilesPresent },
    @{ Name = "Package Restore"; Status = $LASTEXITCODE -eq 0 },
    @{ Name = "Project Build"; Status = $LASTEXITCODE -eq 0 }
)

$passed = 0
$total = $checks.Count

foreach ($check in $checks) {
    if ($check.Status) {
        Write-Host "  PASS: $($check.Name)" -ForegroundColor Green
        $passed++
    } else {
        Write-Host "  FAIL: $($check.Name)" -ForegroundColor Red
    }
}

Write-Host ""
$percentage = [math]::Round(($passed / $total) * 100, 1)
Write-Host "Overall Status: $passed/$total checks passed ($percentage%)" -ForegroundColor White

if ($percentage -eq 100) {
    Write-Host ""
    Write-Host "🎉 PERFECT SETUP - READY TO RUN YOUR MAUI PROJECT!" -ForegroundColor Green
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "1. Open Visual Studio 2022" -ForegroundColor White
    Write-Host "2. Open PermissionProMaui.csproj" -ForegroundColor White
    Write-Host "3. Press F5 to run the application" -ForegroundColor White
} else {
    Write-Host ""
    Write-Host "⚠️  SETUP INCOMPLETE - PLEASE FOLLOW THE SETUP GUIDE" -ForegroundColor Yellow
    Write-Host "See: COMPLETE_SETUP_GUIDE.md for detailed instructions" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "USEFUL COMMANDS:" -ForegroundColor Yellow
Write-Host "===============" -ForegroundColor Yellow
Write-Host "dotnet --version                    # Check .NET version" -ForegroundColor White
Write-Host "dotnet workload list               # List installed workloads" -ForegroundColor White
Write-Host "dotnet workload install maui       # Install MAUI workload" -ForegroundColor White
Write-Host "dotnet restore                     # Restore packages" -ForegroundColor White
Write-Host "dotnet build                       # Build project" -ForegroundColor White
Write-Host "dotnet run --framework net8.0-windows10.0.19041.0  # Run on Windows" -ForegroundColor White 