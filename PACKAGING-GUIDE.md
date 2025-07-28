# Git Version Control GUI - Packaging Guide

This guide explains how to package your Git Version Control GUI application into different types of installers and distributable formats.

## Quick Start - Standalone Executable (Recommended)

The easiest way to distribute your application is as a standalone executable that doesn't require .NET runtime.

### Method 1: Using the Batch File (Easiest)

1. **Run the batch file:**
   ```cmd
   build-simple-standalone.bat
   ```

2. **Find your executable:**
   - Location: `bin\Release\net9.0-windows\win-x64\publish\GitVersionControl.exe`
   - Size: ~183 MB
   - **No .NET runtime required!**

3. **Distribute the single .exe file to users**

### Method 2: Manual Command

```cmd
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Packaging Options

### 1. Standalone Executable (Recommended)

**Pros:**
- No .NET runtime required on target machine
- Single file distribution
- Works on any Windows 10/11 machine
- Easy to distribute

**Cons:**
- Larger file size (~183 MB)
- Slower startup time

**Use when:** Distributing to end users who may not have .NET installed

### 2. Framework-Dependent Executable

**Pros:**
- Smaller file size (~30-50 MB)
- Faster startup time
- Smaller distribution package

**Cons:**
- Requires .NET 9.0 runtime on target machine

**Use when:** Distributing to developers or users who already have .NET installed

```cmd
dotnet publish -c Release -r win-x64 --self-contained false
```

### 3. Ready-to-Run Executable (Best Performance)

**Pros:**
- Fastest startup time
- No .NET runtime required
- Optimized for performance

**Cons:**
- Very long build time (5-10 minutes)
- Larger file size

**Use when:** Performance is critical and you have time for long builds

```cmd
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true
```

## Professional Installer Options

### Option 1: Inno Setup (Recommended for Windows)

1. **Download Inno Setup:** https://jrsoftware.org/isinfo.php
2. **Use the provided script:** `GitVersionControl.iss`
3. **Build the installer:**
   ```cmd
   iscc GitVersionControl.iss
   ```

**Features:**
- Professional Windows installer
- Desktop shortcuts
- Start menu integration
- Uninstall support
- Custom branding

### Option 2: WiX Toolset

1. **Install WiX Toolset:** https://wixtoolset.org/releases/
2. **Use the generated WiX project:** `GitVersionControl.wxs`
3. **Build the installer:**
   ```cmd
   candle GitVersionControl.wxs
   light GitVersionControl.wixobj
   ```

### Option 3: MSIX Package (Modern Windows)

For Windows Store or enterprise distribution:

```cmd
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishProfile=Properties\PublishProfiles\win10-x64.pubxml
```

## Build Scripts Provided

### `build-simple-standalone.bat`
- Creates standalone executable (no .NET required)
- Fast build time
- Recommended for most users

### `build-installer.bat`
- Multiple packaging options
- Interactive menu
- Good for testing different approaches

### `create-installer.ps1`
- PowerShell script with advanced options
- Creates Inno Setup and WiX scripts
- More comprehensive than batch files

## File Locations After Build

### Standalone Executable
```
bin\Release\net9.0-windows\win-x64\publish\GitVersionControl.exe
```

### Framework-Dependent
```
bin\Release\net9.0-windows\win-x64\publish\
├── GitVersionControl.exe
├── GitVersionControl.dll
├── GitVersionControl.deps.json
└── [various .NET DLLs]
```

### Inno Setup Installer
```
Output\GitVersionControl-Setup.exe
```

## Distribution Recommendations

### For End Users (Non-Technical)
1. **Use standalone executable** - No .NET requirement
2. **Package with Inno Setup** - Professional installer experience
3. **Include README** - Basic usage instructions

### For Developers
1. **Use framework-dependent** - Smaller size, faster startup
2. **Provide source code** - Allow customization
3. **Include documentation** - API and usage docs

### For Enterprise
1. **Use MSIX package** - Modern Windows packaging
2. **Code signing** - Required for enterprise deployment
3. **Group policy support** - Centralized management

## Troubleshooting

### Build Errors

**Error: "Windows Forms is not supported with trimming"**
- Solution: Use `-p:PublishTrimmed=false` or remove trimming entirely

**Error: "ReadyToRun compilation failed"**
- Solution: Use the simple standalone build without ReadyToRun

**Error: "Inno Setup not found"**
- Solution: Download and install Inno Setup from https://jrsoftware.org/isinfo.php

### Runtime Errors

**Error: "Missing .NET runtime"**
- Solution: Use standalone executable build

**Error: "Missing dependencies"**
- Solution: Ensure all DLLs are included in distribution

## Code Signing (Optional)

For professional distribution, consider code signing your executable:

1. **Purchase a code signing certificate**
2. **Sign the executable:**
   ```cmd
   signtool sign /f certificate.pfx /p password GitVersionControl.exe
   ```

## File Size Optimization

### Reduce Size with Trimming (Experimental)
```cmd
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true
```
**Warning:** May cause runtime errors with Windows Forms

### Remove Debug Symbols
```cmd
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false
```

## Best Practices

1. **Test on clean machines** - Ensure no .NET runtime is installed
2. **Version your releases** - Use semantic versioning
3. **Include changelog** - Document what's new
4. **Test installer** - Verify uninstall works correctly
5. **Backup before testing** - Installers can modify system

## Quick Commands Reference

```cmd
# Standalone executable (recommended)
build-simple-standalone.bat

# Framework-dependent
dotnet publish -c Release -r win-x64 --self-contained false

# Single file with ReadyToRun (slow build)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true

# Inno Setup installer
iscc GitVersionControl.iss

# Clean build
dotnet clean
dotnet build -c Release
``` 