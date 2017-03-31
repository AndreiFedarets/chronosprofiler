%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild src\Chronos.sln /t:Rebuild /p:Configuration=Release
IF NOT %errorlevel% == 0 pause