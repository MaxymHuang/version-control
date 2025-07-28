@echo off
echo Git Version Control GUI - Installer Builder
echo ==========================================

echo.
echo Choose installer type:
echo 1. Self-contained executable (recommended)
echo 2. Framework-dependent executable
echo 3. Single file executable
echo 4. All options
echo.

set /p choice="Enter your choice (1-4): "

if "%choice%"=="1" goto self-contained
if "%choice%"=="2" goto framework-dependent
if "%choice%"=="3" goto single-file
if "%choice%"=="4" goto all
goto invalid

:self-contained
echo Creating self-contained executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true
echo.
echo Self-contained executable created in: bin\Release\net9.0-windows\win-x64\publish\
echo This includes all dependencies and can run on any Windows machine.
goto end

:framework-dependent
echo Creating framework-dependent executable...
dotnet publish -c Release -r win-x64 --self-contained false
echo.
echo Framework-dependent executable created in: bin\Release\net9.0-windows\win-x64\publish\
echo This requires .NET 9.0 runtime to be installed on the target machine.
goto end

:single-file
echo Creating single file executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
echo.
echo Single file executable created in: bin\Release\net9.0-windows\win-x64\publish\
echo This creates a single .exe file with all dependencies included.
goto end

:all
echo Creating all types of installers...
echo.
echo 1. Self-contained executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true
echo.
echo 2. Framework-dependent executable...
dotnet publish -c Release -r win-x64 --self-contained false
echo.
echo 3. Single file executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
echo.
echo All installers created successfully!
echo Check the bin\Release\net9.0-windows\win-x64\publish\ directory.
goto end

:invalid
echo Invalid choice. Please run the script again and select a valid option.
goto end

:end
echo.
echo Build completed! Check the output directories for your installer files.
pause 