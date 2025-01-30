# CliRunner
CliRunner is a library for interacting with Command Line Interfaces and wrapping around executables.

[![NuGet](https://img.shields.io/nuget/v/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/) 
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/)


## Features
* For .NET 8 and newer TFMs CliRunner has few dependencies ^1
* Dependency Injection extensions to make using it easier.
* Support for specific specializations such as running executables or commands via Windows Powershell or CMD on Windows ^2
* SourceLink support

## Why use CliRunner over [CliWrap](https://github.com/Tyrrrz/CliWrap/)?
* Greater separation of concerns with the Command class (v0.8.0 and newer) - Command Running and Pipe handling are moved to separate classes.
* Supports Dependency Injection and promotes the Single Responsibility Principle
* No hidden or additional licensing terms are required beyond the source code license.
* No imported C code - This library is entirely written in C#.
* No lock in regarding Piping support - Use .NET's StreamWriter and StreamReader classes as inputs and outputs respectively.
* Uses .NET's built in ``Process`` type.

^1 - OSCompatibilityLib dependency and [Polyfill](https://github.com/SimonCropp/Polyfill) are only required for .NET Standard 2.0 and 2.1 users

^2 - Specialization library is distributed separately.

## Supported Platforms 
This can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

| Operating System | Support Status | Notes |
|-|-|-|
| Windows | Fully Supported :white_check_mark: | |
| macOS | Fully Supported :white_check_mark: | |
| Mac Catalyst | Fully Supported :white_check_mark: | |
| Linux | Fully Supported :white_check_mark: | |
| FreeBSD | Fully Supported :white_check_mark: | |
| Android | Untested Platform :warning: | Support for this platform has not been tested but should theoretically work. |
| IOS | Not Supported :x: | Not supported due to ``Process.Start()`` not supporting IOS. ^3 | 
| tvOS | Not Supported :x: | Not supported due to ``Process.Start()`` not supporting tvOS ^3 |
| watchOS | Not Supported :x: | Not supported due to ``Process.Start()`` not supporting watchOS ^4 |
| Browser | Not Supported :x: | Not supported due to not being a valid target Platform for executing programs. |

^3 - See the [Process class documentation](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-9.0#system-diagnostics-process-start) for more info.

^4 - Lack of watchOS support is implied by lack of IOS support since watchOS is based on IOS.


**Note:** This library has not been tested on Android or Tizen!

## Installation
* [Nuget](https://nuget.org/packages/) or ``dotnet add package CliRunner``

## Usage
CliRunner uses a fluent builder style of syntax to easily configure and run Commands.

The following example shows how to configure and build a Command that returns a BufferedCommandResult which contains redirected StandardOutput and StandardError.

```csharp
using CliRunner;
using CliRunner.Builders;
using CliRunner.Extensions;

  /// Initialize CommandRunner with Dependency Injection.
  ServiceCollection services = new ServiceCollection();
  services.UseCliRunner();

  ServiceProvider sp = services.Build();
  ICommandRunner _commandRunner = sp.GetService<ICommandRunner>();

  // Build your Command fluently 
  ICommandBuilder builder = new CommandBuilder("Path/To/Executable")
                            .WithArguments(["arg1", "arg2"])
                            .WithWorkingDirectory("/Path/To/Directory");
  
  Command command = builder.ToCommand();
  
var result = await _commandRunner.ExecuteBufferedAsync(command);
```

## License
CliRunner is licensed under the MPL 2.0 license. If you modify any of CliRunner's files then the modified files must be licensed under the MPL 2.0 .

If you use CliRunner in your project please make an exact copy of the contents of CliRunner's LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.

## Acknowledgements
This project would like to thank the following projects for their work:
* [CliWrap](https://github.com/Tyrrrz/CliWrap/) for inspiring this project
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support

For more information please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/CliRunner/blob/main/THIRD_PARTY_NOTICES.txt).
