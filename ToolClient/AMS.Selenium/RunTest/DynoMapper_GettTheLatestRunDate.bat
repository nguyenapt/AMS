@echo off

::Get current file directory
SET curDir=%cd%
ECHO %curDir%   
:: => G:\Project\MSTest_V2\DynoMapper_Selenium\RunTest


SET nunitConsoleDir=D:\6_Company_Project\AMS_Selenium\RunTest\NUnit_Console\NUnit.Console-3.10.0
ECHO %nunitConsoleDir%

:: get dll directory
cd %curDir%
cd ..
:: => G:\Project\MSTest_V2\DynoMapper_Selenium\
SET projectDir=%cd%
SET dllDir=D:\6_Company_Project\AMS_Selenium\bin\Debug\DynoMapper_Selenium.dll


::Change change directory command
g:
cd %nunitConsoleDir%
::Start a program, command or batch script 
start nunit3-console.exe nunit3-console --test=DynoMapper.GettTheLatestRunDate.gettTheLatestRunDate %dllDir%