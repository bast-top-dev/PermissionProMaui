# Perfect MAUI Migration Validation Script
Write-Host "Validating Perfect MAUI Migration..." -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Green

$validationResults = @()
$errors = @()
$warnings = @()

# Function to check if file exists
function Test-FileExists {
    param([string]$FilePath, [string]$Description)
    
    if (Test-Path $FilePath) {
        $validationResults += "PASS: $Description - Found"
        return $true
    } else {
        $errors += "FAIL: $Description - Missing: $FilePath"
        return $false
    }
}

# Function to check file content
function Test-FileContent {
    param([string]$FilePath, [string]$SearchText, [string]$Description)
    
    if (Test-Path $FilePath) {
        $content = Get-Content $FilePath -Raw
        if ($content -match $SearchText) {
            $validationResults += "PASS: $Description - Valid"
            return $true
        } else {
            $warnings += "WARN: $Description - Content issue: $FilePath"
            return $false
        }
    } else {
        $errors += "FAIL: $Description - File missing: $FilePath"
        return $false
    }
}

# Function to check for syntax errors
function Test-SyntaxErrors {
    param([string]$FilePath, [string]$Description)
    
    if (Test-Path $FilePath) {
        $content = Get-Content $FilePath -Raw
        
        # Check for common C# syntax issues
        $syntaxIssues = @()
        
        # Check for unclosed braces
        $openBraces = ($content.ToCharArray() | Where-Object { $_ -eq '{' }).Count
        $closeBraces = ($content.ToCharArray() | Where-Object { $_ -eq '}' }).Count
        if ($openBraces -ne $closeBraces) {
            $syntaxIssues += "Unmatched braces"
        }
        
        # Check for missing using statements
        if ($content -match "ActivityItem" -and $content -notmatch "using.*Models") {
            $syntaxIssues += "Missing using statement for Models"
        }
        
        if ($syntaxIssues.Count -eq 0) {
            $validationResults += "PASS: $Description - Syntax OK"
            return $true
        } else {
            $errors += "FAIL: $Description - Syntax issues: $($syntaxIssues -join ', ')"
            return $false
        }
    } else {
        $errors += "FAIL: $Description - File missing: $FilePath"
        return $false
    }
}

# Main validation process
try {
    Write-Host "`nChecking project structure..." -ForegroundColor Cyan
    
    # Check essential files
    Test-FileExists "PermissionProMaui.csproj" "Project file"
    Test-FileExists "MauiProgram.cs" "MAUI Program file"
    Test-FileExists "App.xaml" "App XAML"
    Test-FileExists "App.xaml.cs" "App code-behind"
    Test-FileExists "AppShell.xaml" "App Shell"
    
    # Check directories
    Test-FileExists "Views" "Views directory"
    Test-FileExists "ViewModels" "ViewModels directory"
    Test-FileExists "Models" "Models directory"
    Test-FileExists "Services" "Services directory"
    Test-FileExists "Platforms" "Platforms directory"
    Test-FileExists "Resources" "Resources directory"
    
    # Check Resources subdirectories
    Test-FileExists "Resources/Styles" "Styles directory"
    Test-FileExists "Resources/AppIcon" "App Icon directory"
    Test-FileExists "Resources/Splash" "Splash directory"
    Test-FileExists "Resources/Fonts" "Fonts directory"
    
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
    
    Write-Host "`nChecking file content..." -ForegroundColor Cyan
    
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
    
    Write-Host "`nChecking for syntax errors..." -ForegroundColor Cyan
    
    # Check for syntax errors in key files
    Test-SyntaxErrors "ViewModels/HomeViewModel.cs" "HomeViewModel syntax"
    Test-SyntaxErrors "Views/HomePage.xaml.cs" "HomePage code-behind syntax"
    Test-SyntaxErrors "Models/ActivityItem.cs" "ActivityItem model syntax"
    
    # Check for duplicate content issues
    Write-Host "`nChecking for duplicate content..." -ForegroundColor Cyan
    
    $androidManifest = Get-Content "Platforms/Android/AndroidManifest.xml" -Raw
    $manifestCount = ([regex]::Matches($androidManifest, "<manifest")).Count
    if ($manifestCount -gt 1) {
        $warnings += "WARN: Android manifest contains duplicate content"
    } else {
        $validationResults += "PASS: Android manifest - No duplicates"
    }
    
    $iosPlist = Get-Content "Platforms/iOS/Info.plist" -Raw
    $plistCount = ([regex]::Matches($iosPlist, "<plist")).Count
    if ($plistCount -gt 1) {
        $warnings += "WARN: iOS Info.plist contains duplicate content"
    } else {
        $validationResults += "PASS: iOS Info.plist - No duplicates"
    }
    
    # Check for MVVM pattern implementation
    Write-Host "`nChecking MVVM implementation..." -ForegroundColor Cyan
    
    $homeViewModel = Get-Content "ViewModels/HomeViewModel.cs" -Raw
    if ($homeViewModel -match "ObservableObject") {
        $validationResults += "PASS: HomeViewModel uses ObservableObject"
    } else {
        $warnings += "WARN: HomeViewModel should use ObservableObject"
    }
    
    if ($homeViewModel -match "AsyncRelayCommand") {
        $validationResults += "PASS: HomeViewModel uses AsyncRelayCommand"
    } else {
        $warnings += "WARN: HomeViewModel should use AsyncRelayCommand"
    }
    
    if ($homeViewModel -match "using PermissionProMaui.Models") {
        $validationResults += "PASS: HomeViewModel has correct using statements"
    } else {
        $errors += "FAIL: HomeViewModel missing using statement for Models"
    }
    
    # Check for proper binding in XAML
    $homeXaml = Get-Content "Views/HomePage.xaml" -Raw
    if ($homeXaml -match "Binding") {
        $validationResults += "PASS: HomePage uses data binding"
    } else {
        $warnings += "WARN: HomePage should use data binding"
    }
    
    if ($homeXaml -match "NavigateToOpenOrdersCommand") {
        $validationResults += "PASS: Open Orders has proper command binding"
    } else {
        $warnings += "WARN: Open Orders should have proper command binding"
    }
    
    # Check for proper using statements in code-behind
    $homeCodeBehind = Get-Content "Views/HomePage.xaml.cs" -Raw
    if ($homeCodeBehind -match "using Microsoft.Maui.ApplicationModel") {
        $validationResults += "PASS: HomePage code-behind has correct using statements"
    } else {
        $warnings += "WARN: HomePage code-behind missing using statement for ApplicationModel"
    }
    
    # Display results
    Write-Host "`nValidation Results:" -ForegroundColor Green
    Write-Host "==================" -ForegroundColor Green
    
    foreach ($result in $validationResults) {
        Write-Host $result -ForegroundColor Green
    }
    
    if ($warnings.Count -gt 0) {
        Write-Host "`nWarnings:" -ForegroundColor Yellow
        Write-Host "=========" -ForegroundColor Yellow
        foreach ($warning in $warnings) {
            Write-Host $warning -ForegroundColor Yellow
        }
    }
    
    if ($errors.Count -gt 0) {
        Write-Host "`nErrors:" -ForegroundColor Red
        Write-Host "=======" -ForegroundColor Red
        foreach ($errorItem in $errors) {
            Write-Host $errorItem -ForegroundColor Red
        }
    }
    
    # Summary
    $totalChecks = $validationResults.Count + $warnings.Count + $errors.Count
    $successRate = [math]::Round(($validationResults.Count / $totalChecks) * 100, 1)
    
    Write-Host "`nPerfect Migration Status Summary:" -ForegroundColor Cyan
    Write-Host "===================================" -ForegroundColor Cyan
    Write-Host "PASSED: $($validationResults.Count)" -ForegroundColor Green
    Write-Host "WARNINGS: $($warnings.Count)" -ForegroundColor Yellow
    Write-Host "FAILED: $($errors.Count)" -ForegroundColor Red
    Write-Host "SUCCESS RATE: $successRate%" -ForegroundColor Cyan
    
    if ($errors.Count -eq 0) {
        Write-Host "`nSUCCESS: Perfect migration validation completed successfully!" -ForegroundColor Green
        Write-Host "The project is ready for building and testing." -ForegroundColor Green
        Write-Host "All requirements met - No errors found!" -ForegroundColor Green
    } else {
        Write-Host "`nWARNING: Migration has issues that need to be addressed." -ForegroundColor Yellow
        Write-Host "Please fix the errors before proceeding." -ForegroundColor Yellow
    }
    
}
catch {
    Write-Host "ERROR: Validation script error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} 