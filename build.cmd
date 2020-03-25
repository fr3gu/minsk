@echo off

dotnet build
dotnet test .\src\test\Minsk.Test\Minsk.Test.csproj --logger "trx;LogFileName=TestResults.xml"