@echo off
PATH %PATH%;..\packages\OpenCover.4.6.519\tools
PATH %PATH%;..\packages\ReportGenerator.2.4.5.0\tools
PATH %PATH%;"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE"
mkdir coverage >NUL 2>&1
OpenCover.Console.exe -target:MSTest.exe -targetargs:"/nologo /testcontainer:..\MersenneTwister.Tests\bin\Debug\MersenneTwister.Tests.dll" -coverbytest:*.Tests.dll -register:user -output:coverage\results.xml
rmdir /s /q TestResults
ReportGenerator.exe -reports:coverage\results.xml -targetdir:coverage
start coverage\index.htm
