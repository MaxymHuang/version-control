@echo off
echo Git Version Control GUI - Build Script
echo =====================================
echo.

REM Check if dotnet is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo.
    echo Please install .NET 9.0 SDK or later from:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

echo .NET SDK found. Version:
dotnet --version
echo.

echo Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)
echo.

echo Building the application...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)
echo.

echo Build completed successfully!
echo.
echo To run the application:
echo   dotnet run
echo.
echo Or to create a standalone executable:
echo   dotnet publish -c Release -r win-x64 --self-contained
echo.
pause 