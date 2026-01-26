@echo off
echo Starting OtakuTracker Servers...

start "OtakuTracker Backend" cmd /k "dotnet run --project server/Tracker.API/Tracker.API.csproj --launch-profile http"
timeout /t 5

start "OtakuTracker Frontend" cmd /k "cd client && npm run dev"

echo Servers launched in new windows.
echo Backend: http://localhost:5000
echo Frontend: http://localhost:3000
pause
