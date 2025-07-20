@echo off
echo Building TimyxRAMSimulator...
echo.

dotnet build --configuration Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful!
    echo Executable location: bin\Release\net6.0-windows\TimyxRAMSimulator.exe
    echo.
    pause
) else (
    echo.
    echo Build failed!
    echo.
    pause
)