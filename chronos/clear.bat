call:ClearSolutionDir Chronos
call:ClearProjectDir Chronos
call:ClearProjectDir Chronos.Daemon.DataProcessing
call:ClearProjectDir Chronos.Daemon.EntryPoint
call:ClearProjectDir Chronos.Extension.ProfilingStrategy.DotNetExceptionMonitor
call:ClearProjectDir Chronos.Extension.ProfilingStrategy.DotNetPerformance
call:ClearProjectDir Chronos.Extension.ProfilingTarget.ConcreteProcess
call:ClearProjectDir Chronos.Extension.ProfilingTarget.InternetInformationService
call:ClearProjectDir Chronos.Extension.ProfilingTarget.ProcessByName
call:ClearProjectDir Chronos.Host.EntryPoint.Application
call:ClearProjectDir Chronos.Host.EntryPoint.Service
call:ClearProjectDir Chronos.Installer
call:ClearProjectDir Chronos.ProfilerAgent
call:ClearProjectDir Chronos.ProfilerAgent32
call:ClearProjectDir Chronos.ProfilerAgent64
call:ClearProjectDir Chronos.ProcDump32
call:ClearProjectDir Chronos.ProcDump64
call:ClearProjectDir Chronos.Client.Console.EntryPoint
call:ClearProjectDir Chronos.Client.Win.Controls
call:ClearProjectDir Chronos.Client.Win.EntryPoint
call:ClearProjectDir Chronos.Client.Win.Resources
call:ClearProjectDir Chronos.Client.Win.Views
rmdir /s /q %CD%\src\Chronos.ProfilerAgent\output


:ClearProjectDir
rmdir /s /q %CD%\src\%~1\bin
rmdir /s /q %CD%\src\%~1\obj
del /s /q %CD%\src\%~1\%~1.csproj.user
del /s /q %CD%\src\%~1\%~1.csproj.DotSettings.user
del /s /q %CD%\src\%~1\%~1.vcxproj.user
del /s /q %CD%\src\%~1\%~1.vcxproj.DotSettings.user
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