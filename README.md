# CliRunner
CliRunner is a library for interacting with Command Line Interfaces and wrapping around executables.

[![NuGet](https://img.shields.io/nuget/v/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/) 
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/)


## Features
* For .NET 8 and newer TFMs CliRunner is dependency free ^1
* Support for specific specializations such as running executables or commands via Windows Powershell or CMD on Windows ^2
* SourceLink support

## Why use CliRunner over [CliWrap](https://github.com/Tyrrrz/CliWrap/)?
* No hidden or additional licensing terms are required beyond the source code license
* No imported C code - This library is entirely written in C#
* No lock in regarding Piping support

^1 - RuntimeExtensions dependency only required for .NET Standard 2.0 and 2.1 users

^2 - Specialization library is distributed separately.

## Support 
This can be added to any .NET Standard 2.0, .NET Standard 2.1 or .NET 8 supported Application or Library.

## Install
* [Nuget](https://nuget.org/packages/) or ``dotnet add package CliRunner``

## Usage

```csharp
using CliRunner;

var result = await Cli.Run("Path/To/Exe")
              .WithArguments(["arg1", "arg2"])
              .WithWorkingDirectory("/Path/To/Directory")
              .ExecuteBufferedAsync();


``
```

## Licensing
CliRunner is licensed under the MPL 2.0 license. If you modify any of CliRunner's files then the modified files must be licensed under the MPL 2.0 .

If you use CliRunner in your project please make an exact copy of the contents of CliRunner's LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.
