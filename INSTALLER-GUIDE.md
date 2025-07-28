# Git Version Control GUI - Installer Creation Guide

This guide explains how to create different types of installers for the Git Version Control GUI application.

## Quick Start

### Option 1: Self-Contained Executable (Recommended)
```bash
dotnet publish -c Release -r win-x64 --self-contained true
```
This creates a standalone executable that includes all dependencies and can run on any Windows machine without requiring .NET runtime installation.

### Option 2: Single File Executable
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```
This creates a single .exe file (larger size) that contains everything needed to run the application.

### Option 3: Framework-Dependent Executable
```bash
dotnet publish -c Release -r win-x64 --self-contained false
```
This creates a smaller executable that requires .NET 9.0 runtime to be installed on the target machine.

## Installer Types

### 1. Self-Contained Executable (Recommended for Distribution)

**Pros:**
- No external dependencies required
- Works on any Windows machine
- Easy to distribute

**Cons:**
- Larger file size (~150MB)
- Includes all .NET runtime components

**Usage:**
```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

**Output Location:** `bin\Release\net9.0-windows\win-x64\publish\`

### 2. Single File Executable

**Pros:**
- Single .exe file
- No additional files needed
- Easy to distribute

**Cons:**
- Largest file size (~155MB)
- Slower startup time

**Usage:**
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

**Output Location:** `bin\Release\net9.0-windows\win-x64\publish\GitVersionControl.exe`

### 3. Framework-Dependent Executable

**Pros:**
- Smallest file size
- Fast startup

**Cons:**
- Requires .NET 9.0 runtime on target machine
- More complex distribution

**Usage:**
```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

**Output Location:** `bin\Release\net9.0-windows\win-x64\publish\`

## Professional Installers

### 1. Inno Setup Installer

**Requirements:**
- Download and install Inno Setup from: https://jrsoftware.org/isinfo.php

**Steps:**
1. Build the self-contained executable:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true
   ```

2. Run Inno Setup Compiler:
   ```bash
   iscc GitVersionControl.iss
   ```

3. The installer will be created in the `Output` directory.

**Features:**
- Professional Windows installer
- Desktop and Start Menu shortcuts
- Uninstall support
- Modern wizard interface

### 2. WiX Toolset Installer

**Requirements:**
- Download and install WiX Toolset from: https://wixtoolset.org/releases/

**Steps:**
1. Install WiX Toolset
2. Build the self-contained executable
3. Use the provided WiX project file (GitVersionControl.wxs)
4. Build with WiX compiler

**Features:**
- Advanced Windows installer
- Custom actions support
- MSI package format
- Enterprise deployment ready

### 3. MSIX Package (Modern Windows)

**Requirements:**
- Windows 10/11
- Microsoft Store Developer Account (for Store distribution)

**Steps:**
1. Update the project file with MSIX properties
2. Build the MSIX package:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained -p:PublishProfile=Properties\PublishProfiles\win10-x64.pubxml
   ```

**Features:**
- Modern Windows packaging
- Automatic updates
- Sandboxed execution
- Store distribution ready

## Automated Build Scripts

### Using the Batch File
Run `build-installer.bat` and choose your preferred installer type:
1. Self-contained executable
2. Framework-dependent executable
3. Single file executable
4. All options

### Using the PowerShell Script
Run `create-installer.ps1` for advanced installer creation options:
1. Self-contained executable
2. MSIX package
3. Inno Setup installer
4. WiX installer
5. All options

## Distribution Options

### 1. Direct Distribution
- Copy the `publish` folder contents
- Users can run `GitVersionControl.exe` directly
- No installation required

### 2. ZIP Package
- Create a ZIP file containing the publish folder
- Users extract and run the executable
- Simple distribution method

### 3. Professional Installer
- Use Inno Setup or WiX to create .exe or .msi installers
- Professional installation experience
- Automatic shortcuts and uninstall

### 4. Microsoft Store
- Package as MSIX
- Submit to Microsoft Store
- Automatic updates and distribution

## File Size Comparison

| Type | Size | Dependencies |
|------|------|--------------|
| Self-contained | ~150MB | None |
| Single file | ~155MB | None |
| Framework-dependent | ~30MB | .NET 9.0 Runtime |
| Inno Setup installer | ~150MB | None |
| MSIX package | ~150MB | Windows 10/11 |

## Troubleshooting

### Common Issues

1. **"Windows Forms is not supported with trimming"**
   - Solution: Remove `-p:PublishTrimmed=true` from the command

2. **Missing dependencies**
   - Solution: Use `--self-contained true` to include all dependencies

3. **Large file size**
   - Solution: Use framework-dependent build for smaller size (requires .NET runtime)

4. **Inno Setup not found**
   - Solution: Install Inno Setup from the official website

5. **WiX compilation errors**
   - Solution: Ensure WiX Toolset is properly installed and configured

### Performance Tips

1. **For faster startup:** Use framework-dependent builds
2. **For smaller distribution:** Use single file with trimming (if possible)
3. **For enterprise deployment:** Use WiX MSI packages
4. **For modern Windows:** Use MSIX packages

## Security Considerations

1. **Code signing:** Sign your executables for better security
2. **Digital certificates:** Obtain a code signing certificate
3. **Antivirus compatibility:** Test with major antivirus software
4. **Windows SmartScreen:** Submit for reputation building

## Best Practices

1. **Version management:** Update version numbers in project files
2. **Release notes:** Include release notes with installers
3. **Testing:** Test installers on clean virtual machines
4. **Documentation:** Provide installation and usage instructions
5. **Updates:** Plan for future update mechanisms

## Next Steps

1. Choose your preferred installer type based on your distribution needs
2. Customize the installer scripts with your company information
3. Test the installers on target systems
4. Set up automated build processes
5. Plan for updates and maintenance

For more information, refer to the main README.md file or the .NET documentation. 