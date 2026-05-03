@echo off
net session >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Requesting administrative privileges...
    powershell -Command "Start-Process -FilePath 'cmd.exe' -ArgumentList '/c \"\"%~f0\"\"' -Verb RunAs"
    exit /b
)

echo Starting Asus Fan Control as LocalSystem...
"%~dp0PsExec64.exe" -accepteula -i -s -d "%~dp0AsusFanControlGUI.exe"
echo Done. Asus Fan Control is now running.
