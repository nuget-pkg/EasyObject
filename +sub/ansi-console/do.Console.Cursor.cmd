@echo off
set script_dir=%~dp0
set "script_dir=%script_dir:\=/%"
set str=%1
if "%str:~0,1%"=="@" (
  if "%str%"=="@run" (
    bash.exe -c "%script_dir%/.r.Console.Cursor.sh %2 %3 %4 %5 %6 %7 %8 %9
  ) else if "%str%"=="@exe" (
    if not exist "%script_dir%/Console.Cursor.exe" (
      wingen %script_dir%/Console.Cursor.main.cs
      bash.exe -c "%script_dir%/.r.Console.Cursor.sh @merge -f"
    )
    "%script_dir%/Console.Cursor.exe" %2 %3 %4 %5 %6 %7 %8 %9
  ) else if "%str%"=="@bin" (
    if not exist "%script_dir%/Console.Cursor.exe" (
      wingen %script_dir%/Console.Cursor.main.cs
      bash.exe -c "%script_dir%/.r.Console.Cursor.sh @pack -f"
    )
    "%script_dir%/Console.Cursor.exe" %2 %3 %4 %5 %6 %7 %8 %9
  ) else (
    bash.exe -c "%script_dir%/.r.Console.Cursor.sh %*"
  )
) else (
  cscs -nuget:restore "%script_dir%/Console.Cursor.main.cs">NUL 2>&1
  cscs -l:0 "%script_dir%/Console.Cursor.main.cs" %*
)
