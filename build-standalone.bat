@echo off
echo Git Version Control GUI - Standalone Executable Builder
echo =====================================================

echo.
echo Building standalone executable that doesn't require .NET runtime...
echo.

REM Clean previous builds
if exist "bin\Release\net9.0-windows\win-x64\publish" rmdir /s /q "bin\Release\net9.0-windows\win-x64\publish"

REM Build the standalone executable
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=false

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo BUILD SUCCESSFUL!
    echo ========================================
    echo.
    echo Standalone executable created at:
    echo bin\Release\net9.0-windows\win-x64\publish\GitVersionControl.exe
    echo.
    echo This executable:
    echo - Does NOT require .NET runtime to be installed
    echo - Includes all necessary dependencies
    echo - Can run on any Windows 10/11 machine
    echo - File size: approximately 150-200 MB
    echo.
    echo You can distribute this single .exe file to users.
    echo.
) else (
    echo.
    echo ========================================
    echo BUILD FAILED!
    echo ========================================
    echo.
    echo Please check the error messages above.
    echo.
)

pause 