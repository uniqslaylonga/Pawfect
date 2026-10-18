@echo off
rem Run on the PC that has your Pawfect data. Creates Pawfect.bak in this folder.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Backup-Database.ps1"
pause
