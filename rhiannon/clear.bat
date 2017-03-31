call:ClearSolutionDir Rhiannon
call:ClearProjectDir Rhiannon
call:ClearProjectDir Rhiannon.Ribbon
call:ClearProjectDir Rhiannon.Windows


:ClearProjectDir
rmdir /s /q %CD%\src\%~1\bin
rmdir /s /q %CD%\src\%~1\obj
del /s /q %CD%\src\%~1\%~1.csproj.user
del /s /q %CD%\src\%~1\%~1.csproj.DotSettings.user
goto:eof

:ClearSolutionDir
rmdir /s /q %CD%\build
rmdir /s /q %CD%\msi
rmdir /s /q %CD%\src\ipch
rmdir /s /q %CD%\src\_ReSharper.%~1
del /s /q %CD%\src\%~1.5.0.ReSharper.user
del /s /q %CD%\src\%~1.sln.DotSettings.user
del /s /q %CD%\src\%~1.suo
del /s /q %CD%\src\%~1.sdf
goto:eof