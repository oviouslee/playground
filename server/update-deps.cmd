@echo off

echo Updating dbseeder dependencies...
cd .\dbseeder
call dotnet add package Microsoft.EntityFrameworkCore.Relational
call dotnet add package Microsoft.EntityFrameworkCore.SqlServer

echo Updating Playground.Core dependencies...
cd ..\Playground.Core
call dotnet add package Microsoft.EntityFrameworkCore
call dotnet add package Newtonsoft.Json

echo Updating Playground.Auth dependencies...
cd ..\Playground.Auth
call dotnet add package Microsoft.EntityFrameworkCore

echo Updating Playground.Data dependencies...
cd ..\Playground.Data
call dotnet add package Microsoft.EntityFrameworkCore.SqlServer
call dotnet add package Microsoft.EntityFrameworkCore.Tools
call dotnet add package Newtonsoft.Json

echo Updating Playground.Identity dependencies...
cd ..\Playground.Identity
call dotnet add package Microsoft.Extensions.Configuration.Abstractions
call dotnet add package Microsoft.Extensions.Configuration.Binder
call dotnet add package System.DirectoryServices
call dotnet add package System.DirectoryServices.AccountManagement

echo Updating Playground.Identity.Mock dependencies...
cd ..\Playground.Identity.Mock

echo Updating Playground.Office dependencies...
cd ..\Playground.Office
call dotnet add package DocumentFormat.OpenXml

echo Updating Playground.Sql dependencies...
cd ..\Playground.Sql
call dotnet add package Microsoft.Data.SqlClient
call dotnet add package Newtonsoft.Json

echo Updating Playground.Web dependencies...
cd ..\Playground.Web
call dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
call dotnet add package Microsoft.EntityFrameworkCore.Design

echo Caching NuGet dependencies...
cd ..\
call dotnet restore --packages nuget-packages

cd ..
echo Dependencies successfully updated!
