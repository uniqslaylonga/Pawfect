@echo off
rem Run on the employee PC. Asks for administrator permission, then sets up the shared database.
net session >nul 2>&1
if %errorlevel% neq 0 (
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Setup-Server.ps1"
pause
