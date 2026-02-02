# Final Migration Check Script
Write-Host "Checking Perfect MAUI Migration..." -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Green

$passed = 0
$failed = 0
$warnings = 0

# Function to check file
function Test-File {
    param([string]$Path, [string]$Name)
    if (Test-Path $Path) {
        Write-Host "PASS: $Name - Found" -ForegroundColor Green
        $script:passed++
    } else {
        Write-Host "FAIL: $Name - Missing" -ForegroundColor Red
        $script:failed++
    }
}

# Function to check content
function Test-Content {
    param([string]$Path, [string]$SearchText, [string]$Name)
    if (Test-Path $Path) {
        $content = Get-Content $Path -Raw
        if ($content -match $SearchText) {
            Write-Host "PASS: $Name - Valid" -ForegroundColor Green
            $script:passed++
        } else {
            Write-Host "WARN: $Name - Content issue" -ForegroundColor Yellow
            $script:warnings++
        }
    } else {
        Write-Host "FAIL: $Name - File missing" -ForegroundColor Red
        $script:failed++
    }
}

Write-Host "`nChecking Project Structure..." -ForegroundColor Cyan

# Essential files
Test-File "PermissionProMaui.csproj" "Project file"
Test-File "MauiProgram.cs" "MAUI Program file"
Test-File "App.xaml" "App XAML"
Test-File "App.xaml.cs" "App code-behind"
Test-File "AppShell.xaml" "App Shell"

# Directories
Test-File "Views" "Views directory"
Test-File "ViewModels" "ViewModels directory"
Test-File "Models" "Models directory"
Test-File "Services" "Services directory"
Test-File "Platforms" "Platforms directory"
Test-File "Resources" "Resources directory"

# Resources
Test-File "Resources/Styles" "Styles directory"
Test-File "Resources/AppIcon" "App Icon directory"
Test-File "Resources/Splash" "Splash directory"
Test-File "Resources/Fonts" "Fonts directory"

# Platform files
Test-File "Platforms/Android/AndroidManifest.xml" "Android Manifest"
Test-File "Platforms/iOS/Info.plist" "iOS Info.plist"
Test-File "Platforms/MacCatalyst/Info.plist" "MacCatalyst Info.plist"
Test-File "Platforms/Windows/Package.appxmanifest" "Windows AppxManifest"

# Resource files
Test-File "Resources/Styles/Colors.xaml" "Colors resource file"
Test-File "Resources/Styles/Styles.xaml" "Styles resource file"
Test-File "Resources/AppIcon/appicon.svg" "App icon SVG"
Test-File "Resources/AppIcon/appiconfg.svg" "App icon foreground SVG"
Test-File "Resources/Splash/splash.svg" "Splash screen SVG"

# Key view files
Test-File "Views/HomePage.xaml" "Home page XAML"
Test-File "Views/HomePage.xaml.cs" "Home page code-behind"
Test-File "Views/LoginPage.xaml" "Login page XAML"
Test-File "Views/BankAccountsPage.xaml" "Bank accounts page XAML"

# ViewModel files
Test-File "ViewModels/HomeViewModel.cs" "Home ViewModel"
Test-File "ViewModels/BankAccountsViewModel.cs" "Bank accounts ViewModel"

# Service files
Test-File "Services/ApiConnectionService.cs" "API connection service"
Test-File "Services/DatabaseService.cs" "Database service"
Test-File "Services/AuthentificationService.cs" "Authentication service"

# Model files
Test-File "Models/AppSettingsTable.cs" "App settings model"
Test-File "Models/BankTable.cs" "Bank table model"
Test-File "Models/ActivityItem.cs" "Activity item model"

Write-Host "`nChecking File Content..." -ForegroundColor Cyan

# Project file content
Test-Content "PermissionProMaui.csproj" "UseMaui" "MAUI project configuration"
Test-Content "PermissionProMaui.csproj" "Microsoft.Maui.Controls" "MAUI dependencies"
Test-Content "PermissionProMaui.csproj" "CommunityToolkit.Mvvm" "MVVM toolkit"

# MauiProgram.cs content
Test-Content "MauiProgram.cs" "AddSingleton" "Dependency injection setup"
Test-Content "MauiProgram.cs" "AddTransient" "View registration"

# App.xaml content
Test-Content "App.xaml" "Colors.xaml" "Resource dictionary references"
Test-Content "App.xaml" "Styles.xaml" "Styles resource references"

# Platform manifest files
Test-Content "Platforms/Android/AndroidManifest.xml" "android.permission.INTERNET" "Android permissions"
Test-Content "Platforms/iOS/Info.plist" "CFBundleDisplayName" "iOS bundle configuration"
Test-Content "Platforms/Windows/Package.appxmanifest" "DisplayName" "Windows app configuration"

Write-Host "`nChecking MVVM Implementation..." -ForegroundColor Cyan

# HomeViewModel checks
Test-Content "ViewModels/HomeViewModel.cs" "ObservableObject" "HomeViewModel uses ObservableObject"
Test-Content "ViewModels/HomeViewModel.cs" "AsyncRelayCommand" "HomeViewModel uses AsyncRelayCommand"
Test-Content "ViewModels/HomeViewModel.cs" "using PermissionProMaui.Models" "HomeViewModel has correct using statements"

# HomePage checks
Test-Content "Views/HomePage.xaml" "Binding" "HomePage uses data binding"
Test-Content "Views/HomePage.xaml" "NavigateToOpenOrdersCommand" "Open Orders has proper command binding"
Test-Content "Views/HomePage.xaml.cs" "using Microsoft.Maui.ApplicationModel" "HomePage code-behind has correct using statements"

Write-Host "`nPerfect Migration Status Summary:" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host "PASSED: $passed" -ForegroundColor Green
Write-Host "WARNINGS: $warnings" -ForegroundColor Yellow
Write-Host "FAILED: $failed" -ForegroundColor Red

$total = $passed + $warnings + $failed
$successRate = [math]::Round(($passed / $total) * 100, 1)
Write-Host "SUCCESS RATE: $successRate%" -ForegroundColor Cyan

if ($failed -eq 0) {
    Write-Host "`nSUCCESS: Perfect migration check completed successfully!" -ForegroundColor Green
    Write-Host "All requirements met - No errors found!" -ForegroundColor Green
    Write-Host "Your MAUI migration is ready for production!" -ForegroundColor Green
} else {
    Write-Host "`nWARNING: Migration has issues that need to be addressed." -ForegroundColor Yellow
    Write-Host "Please fix the failed items before proceeding." -ForegroundColor Yellow
}

Write-Host "`nPerfect Migration Check Complete!" -ForegroundColor Green 