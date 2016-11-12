@echo off

IF EXIST C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe (
  C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Mixter.sln /verbosity:m
) ELSE IF EXIST C:\Windows\Microsoft.NET\Framework\v4.0.30128\MSBuild.exe (
  C:\Windows\Microsoft.NET\Framework\v4.0.30128\MSBuild.exe Mixter.sln /verbosity:m
)

packages\xunit.runner.console.2.1.0\tools\xunit.console Mixter.Domain.Tests\bin\Debug\Mixter.Domain.Tests.dll Mixter.Infrastructure.Tests\bin\Debug\Mixter.Infrastructure.Tests.dll

