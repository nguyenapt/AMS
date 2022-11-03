@echo off

::Get current file directory
SET curDir=%cd%
ECHO %curDir%   
:: => G:\Project\MSTest_V2\DynoMapper_Selenium\RunTest


SET nunitConsoleDir=%curDir%\NUnit_Console\NUnit.Console-3.10.0\bin\net35
ECHO %nunitConsoleDir%

:: get dll directory
cd %curDir%
cd ..
:: => G:\Project\MSTest_V2\DynoMapper_Selenium\
SET projectDir=%cd%
SET dllDir=%projectDir%\bin\Debug\DynoMapper_Selenium.dll


::Change change directory command
g:
cd %nunitConsoleDir%
::Start a program, command or batch script 
start nunit3-console.exe nunit3-console --test=DynoMapper.GetListingProjectsAndSaveToExcelTest.getListingProjectsAndSaveToExcel %dllDir%