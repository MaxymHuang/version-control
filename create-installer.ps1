# Git Version Control GUI Installer Creation Script
# This script creates different types of installers for the application

Write-Host "Git Version Control GUI - Installer Creation Script" -ForegroundColor Green
Write-Host "=================================================" -ForegroundColor Green

# Function to create a simple logo if it doesn't exist
function Create-Logo {
    if (-not (Test-Path "Assets\StoreLogo.png")) {
        Write-Host "Creating placeholder logo..." -ForegroundColor Yellow
        
        # Create a simple text-based logo using PowerShell
        $logoContent = @"
        <svg width="50" height="50" xmlns="http://www.w3.org/2000/svg">
            <rect width="50" height="50" fill="#2D3748"/>
            <text x="25" y="30" font-family="Arial" font-size="12" fill="white" text-anchor="middle">Git</text>
            <text x="25" y="42" font-family="Arial" font-size="8" fill="white" text-anchor="middle">GUI</text>
        </svg>
"@
        
        # For now, we'll create a simple text file as placeholder
        $logoContent | Out-File -FilePath "Assets\logo-placeholder.txt" -Encoding UTF8
        Write-Host "Placeholder logo created. Please replace with actual logo image." -ForegroundColor Yellow
    }
}

# Function to create MSIX package
function Create-MSIXPackage {
    Write-Host "Creating MSIX package..." -ForegroundColor Cyan
    
    try {
        # Build the project
        dotnet build -c Release
        
        # Create MSIX package
        dotnet publish -c Release -r win-x64 --self-contained -p:PublishProfile=Properties\PublishProfiles\win10-x64.pubxml
        
        Write-Host "MSIX package created successfully!" -ForegroundColor Green
        Write-Host "Package location: bin\Release\net9.0-windows\win-x64\publish\" -ForegroundColor Cyan
    }
    catch {
        Write-Host "Error creating MSIX package: $_" -ForegroundColor Red
    }
}

# Function to create traditional installer using WiX
function Create-WiXInstaller {
    Write-Host "Creating WiX installer..." -ForegroundColor Cyan
    
    try {
        # Create WiX project file
        $wixContent = @"
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
    <Product Id="*" 
             Name="Git Version Control GUI" 
             Language="1033" 
             Version="1.0.0.0" 
             Manufacturer="Your Company" 
             UpgradeCode="PUT-GUID-HERE">
        
        <Package InstallerVersion="200" Compressed="yes" InstallScope="perMachine" />
        <MajorUpgrade DowngradeErrorMessage="A newer version of [ProductName] is already installed." />
        <MediaTemplate EmbedCab="yes" />
        
        <Feature Id="ProductFeature" Title="Git Version Control GUI" Level="1">
            <ComponentGroupRef Id="ProductComponents" />
        </Feature>
    </Product>
    
    <Fragment>
        <Directory Id="TARGETDIR" Name="SourceDir">
            <Directory Id="ProgramFilesFolder">
                <Directory Id="INSTALLFOLDER" Name="GitVersionControl" />
            </Directory>
        </Directory>
    </Fragment>
    
    <Fragment>
        <ComponentGroup Id="ProductComponents" Directory="INSTALLFOLDER">
            <!-- Add your application files here -->
        </ComponentGroup>
    </Fragment>
</Wix>
"@
        
        $wixContent | Out-File -FilePath "GitVersionControl.wxs" -Encoding UTF8
        
        Write-Host "WiX project file created. Install WiX Toolset to build the installer." -ForegroundColor Yellow
        Write-Host "Download from: https://wixtoolset.org/releases/" -ForegroundColor Cyan
    }
    catch {
        Write-Host "Error creating WiX installer: $_" -ForegroundColor Red
    }
}

# Function to create self-contained executable
function Create-SelfContainedExecutable {
    Write-Host "Creating self-contained executable..." -ForegroundColor Cyan
    
    try {
        # Create self-contained executable
        dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
        
        Write-Host "Self-contained executable created successfully!" -ForegroundColor Green
        Write-Host "Location: bin\Release\net9.0-windows\win-x64\publish\GitVersionControl.exe" -ForegroundColor Cyan
    }
    catch {
        Write-Host "Error creating self-contained executable: $_" -ForegroundColor Red
    }
}

# Function to create Inno Setup installer
function Create-InnoSetupInstaller {
    Write-Host "Creating Inno Setup installer..." -ForegroundColor Cyan
    
    try {
        # Create Inno Setup script
        $innoContent = @"
[Setup]
AppName=Git Version Control GUI
AppVersion=1.0.0
DefaultDirName={pf}\GitVersionControl
DefaultGroupName=Git Version Control GUI
OutputDir=Output
OutputBaseFilename=GitVersionControl-Setup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Files]
Source: "bin\Release\net9.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Git Version Control GUI"; Filename: "{app}\GitVersionControl.exe"
Name: "{commondesktop}\Git Version Control GUI"; Filename: "{app}\GitVersionControl.exe"

[Run]
Filename: "{app}\GitVersionControl.exe"; Description: "Launch Git Version Control GUI"; Flags: postinstall nowait skipifsilent
"@
        
        $innoContent | Out-File -FilePath "GitVersionControl.iss" -Encoding UTF8
        
        Write-Host "Inno Setup script created: GitVersionControl.iss" -ForegroundColor Green
        Write-Host "Install Inno Setup and run: iscc GitVersionControl.iss" -ForegroundColor Cyan
        Write-Host "Download Inno Setup from: https://jrsoftware.org/isinfo.php" -ForegroundColor Yellow
    }
    catch {
        Write-Host "Error creating Inno Setup installer: $_" -ForegroundColor Red
    }
}

# Main script execution
Write-Host "Choose installer type:" -ForegroundColor Yellow
Write-Host "1. Self-contained executable (recommended for simple distribution)" -ForegroundColor White
Write-Host "2. MSIX package (modern Windows packaging)" -ForegroundColor White
Write-Host "3. Inno Setup installer (traditional Windows installer)" -ForegroundColor White
Write-Host "4. WiX installer (advanced Windows installer)" -ForegroundColor White
Write-Host "5. All options" -ForegroundColor White

$choice = Read-Host "Enter your choice (1-5)"

# Create logo first
Create-Logo

switch ($choice) {
    "1" { Create-SelfContainedExecutable }
    "2" { Create-MSIXPackage }
    "3" { Create-InnoSetupInstaller }
    "4" { Create-WiXInstaller }
    "5" { 
        Create-SelfContainedExecutable
        Create-MSIXPackage
        Create-InnoSetupInstaller
        Create-WiXInstaller
    }
    default { Write-Host "Invalid choice. Exiting." -ForegroundColor Red }
}

Write-Host "`nInstaller creation completed!" -ForegroundColor Green
Write-Host "Check the output directories for your installer files." -ForegroundColor Cyan 