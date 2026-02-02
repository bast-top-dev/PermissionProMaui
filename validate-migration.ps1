# PermissionPro MAUI Migration Validation Script
# This script validates the migration completeness and identifies issues

Write-Host "🔍 Validating PermissionPro MAUI Migration..." -ForegroundColor Green

$validationResults = @()
$errors = @()
$warnings = @()

# Function to check if file exists
function Test-FileExists {
    param([string]$FilePath, [string]$Description)
    
    if (Test-Path $FilePath) {
        $validationResults += "✅ $Description - Found"
        return $true
    } else {
        $errors += "❌ $Description - Missing: $FilePath"
        return $false
    }
}

# Function to check if directory exists
function Test-DirectoryExists {
    param([string]$DirectoryPath, [string]$Description)
    
    if (Test-Path $DirectoryPath) {
        $validationResults += "✅ $Description - Found"
        return $true
    } else {
        $errors += "❌ $Description - Missing: $DirectoryPath"
        return $false
    }
}

# Function to check file content
function Test-FileContent {
    param([string]$FilePath, [string]$SearchText, [string]$Description)
    
    if (Test-Path $FilePath) {
        $content = Get-Content $FilePath -Raw
        if ($content -match $SearchText) {
            $validationResults += "✅ $Description - Valid"
            return $true
        } else {
            $warnings += "⚠️ $Description - Content issue: $FilePath"
            return $false
        }
    } else {
        $errors += "❌ $Description - File missing: $FilePath"
        return $false
    }
}

# Main validation process
try {
    Write-Host "📁 Checking project structure..." -ForegroundColor Cyan
    
    # Check essential files
    Test-FileExists "PermissionProMaui.csproj" "Project file"
    Test-FileExists "MauiProgram.cs" "MAUI Program file"
    Test-FileExists "App.xaml" "App XAML"
    Test-FileExists "App.xaml.cs" "App code-behind"
    Test-FileExists "AppShell.xaml" "App Shell"
    
    # Check directories
    Test-DirectoryExists "Views" "Views directory"
    Test-DirectoryExists "ViewModels" "ViewModels directory"
    Test-DirectoryExists "Models" "Models directory"
    Test-DirectoryExists "Services" "Services directory"
    Test-DirectoryExists "Platforms" "Platforms directory"
    Test-DirectoryExists "Resources" "Resources directory"
    
    # Check Resources subdirectories
    Test-DirectoryExists "Resources/Styles" "Styles directory"
    Test-DirectoryExists "Resources/AppIcon" "App Icon directory"
    Test-DirectoryExists "Resources/Splash" "Splash directory"
    Test-DirectoryExists "Resources/Fonts" "Fonts directory"
    
    # Check platform-specific files
    Test-FileExists "Platforms/Android/AndroidManifest.xml" "Android Manifest"
    Test-FileExists "Platforms/iOS/Info.plist" "iOS Info.plist"
    Test-FileExists "Platforms/MacCatalyst/Info.plist" "MacCatalyst Info.plist"
    Test-FileExists "Platforms/Windows/Package.appxmanifest" "Windows AppxManifest"
    
    # Check resource files
    Test-FileExists "Resources/Styles/Colors.xaml" "Colors resource file"
    Test-FileExists "Resources/Styles/Styles.xaml" "Styles resource file"
    Test-FileExists "Resources/AppIcon/appicon.svg" "App icon SVG"
    Test-FileExists "Resources/AppIcon/appiconfg.svg" "App icon foreground SVG"
    Test-FileExists "Resources/Splash/splash.svg" "Splash screen SVG"
    
    # Check key view files
    Test-FileExists "Views/HomePage.xaml" "Home page XAML"
    Test-FileExists "Views/HomePage.xaml.cs" "Home page code-behind"
    Test-FileExists "Views/LoginPage.xaml" "Login page XAML"
    Test-FileExists "Views/BankAccountsPage.xaml" "Bank accounts page XAML"
    
    # Check ViewModel files
    Test-FileExists "ViewModels/HomeViewModel.cs" "Home ViewModel"
    Test-FileExists "ViewModels/BankAccountsViewModel.cs" "Bank accounts ViewModel"
    
    # Check service files
    Test-FileExists "Services/ApiConnectionService.cs" "API connection service"
    Test-FileExists "Services/DatabaseService.cs" "Database service"
    Test-FileExists "Services/AuthentificationService.cs" "Authentication service"
    
    # Check model files
    Test-FileExists "Models/AppSettingsTable.cs" "App settings model"
    Test-FileExists "Models/BankTable.cs" "Bank table model"
    Test-FileExists "Models/ActivityItem.cs" "Activity item model"
    
    Write-Host "📋 Checking file content..." -ForegroundColor Cyan
    
    # Check project file content
    Test-FileContent "PermissionProMaui.csproj" "UseMaui" "MAUI project configuration"
    Test-FileContent "PermissionProMaui.csproj" "Microsoft.Maui.Controls" "MAUI dependencies"
    Test-FileContent "PermissionProMaui.csproj" "CommunityToolkit.Mvvm" "MVVM toolkit"
    
    # Check MauiProgram.cs content
    Test-FileContent "MauiProgram.cs" "AddSingleton" "Dependency injection setup"
    Test-FileContent "MauiProgram.cs" "AddTransient" "View registration"
    
    # Check App.xaml content
    Test-FileContent "App.xaml" "Colors.xaml" "Resource dictionary references"
    Test-FileContent "App.xaml" "Styles.xaml" "Styles resource references"
    
    # Check platform manifest files
    Test-FileContent "Platforms/Android/AndroidManifest.xml" "android.permission.INTERNET" "Android permissions"
    Test-FileContent "Platforms/iOS/Info.plist" "CFBundleDisplayName" "iOS bundle configuration"
    Test-FileContent "Platforms/Windows/Package.appxmanifest" "DisplayName" "Windows app configuration"
    
    # Check for duplicate content issues
    Write-Host "🔍 Checking for duplicate content..." -ForegroundColor Cyan
    
    $androidManifest = Get-Content "Platforms/Android/AndroidManifest.xml" -Raw
    $manifestCount = ([regex]::Matches($androidManifest, "<manifest")).Count
    if ($manifestCount -gt 1) {
        $warnings += "⚠️ Android manifest contains duplicate content"
    } else {
        $validationResults += "✅ Android manifest - No duplicates"
    }
    
    $iosPlist = Get-Content "Platforms/iOS/Info.plist" -Raw
    $plistCount = ([regex]::Matches($iosPlist, "<plist")).Count
    if ($plistCount -gt 1) {
        $warnings += "⚠️ iOS Info.plist contains duplicate content"
    } else {
        $validationResults += "✅ iOS Info.plist - No duplicates"
    }
    
    # Check for MVVM pattern implementation
    Write-Host "🏗️ Checking MVVM implementation..." -ForegroundColor Cyan
    
    $homeViewModel = Get-Content "ViewModels/HomeViewModel.cs" -Raw
    if ($homeViewModel -match "ObservableObject") {
        $validationResults += "✅ HomeViewModel uses ObservableObject"
    } else {
        $warnings += "⚠️ HomeViewModel should use ObservableObject"
    }
    
    if ($homeViewModel -match "AsyncRelayCommand") {
        $validationResults += "✅ HomeViewModel uses AsyncRelayCommand"
    } else {
        $warnings += "⚠️ HomeViewModel should use AsyncRelayCommand"
    }
    
    # Check for proper binding in XAML
    $homeXaml = Get-Content "Views/HomePage.xaml" -Raw
    if ($homeXaml -match "Binding") {
        $validationResults += "✅ HomePage uses data binding"
    } else {
        $warnings += "⚠️ HomePage should use data binding"
    }
    
    # Display results
    Write-Host "`n📊 Validation Results:" -ForegroundColor Green
    Write-Host "=====================" -ForegroundColor Green
    
    foreach ($result in $validationResults) {
        Write-Host $result -ForegroundColor Green
    }
    
    if ($warnings.Count -gt 0) {
        Write-Host "`n⚠️ Warnings:" -ForegroundColor Yellow
        Write-Host "===========" -ForegroundColor Yellow
        foreach ($warning in $warnings) {
            Write-Host $warning -ForegroundColor Yellow
        }
    }
    
    if ($errors.Count -gt 0) {
        Write-Host "`n❌ Errors:" -ForegroundColor Red
        Write-Host "========" -ForegroundColor Red
        foreach ($errorItem in $errors) {
            Write-Host $errorItem -ForegroundColor Red
        }
    }
    
    # Summary
    $totalChecks = $validationResults.Count + $warnings.Count + $errors.Count
    $successRate = [math]::Round(($validationResults.Count / $totalChecks) * 100, 1)
    
    Write-Host "`n📈 Migration Status Summary:" -ForegroundColor Cyan
    Write-Host "============================" -ForegroundColor Cyan
    Write-Host "✅ Passed: $($validationResults.Count)" -ForegroundColor Green
    Write-Host "⚠️ Warnings: $($warnings.Count)" -ForegroundColor Yellow
    Write-Host "❌ Errors: $($errors.Count)" -ForegroundColor Red
    Write-Host "📊 Success Rate: $successRate%" -ForegroundColor Cyan
    
    if ($errors.Count -eq 0) {
        Write-Host "`n🎉 Migration validation completed successfully!" -ForegroundColor Green
        Write-Host "The project is ready for building and testing." -ForegroundColor Green
    } else {
        Write-Host "`n⚠️ Migration has issues that need to be addressed." -ForegroundColor Yellow
        Write-Host "Please fix the errors before proceeding." -ForegroundColor Yellow
    }
    
}
catch {
    Write-Host "❌ Validation script error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} 