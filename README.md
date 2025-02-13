# CliRunner
CliRunner is a library for interacting with Command Line Interfaces and wrapping around executables.

[![NuGet](https://img.shields.io/nuget/v/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/) 
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/)


## Features
* Promotes the single responsibility principle and separation of concerns
* For .NET 8 and newer TFMs CliRunner has few dependencies.
* Compatible with .NET Standard 2.0 and 2.1 ^1
* Dependency Injection extensions to make using it easier.
* Support for specific specializations such as running executables or commands via Windows Powershell or CMD on Windows ^2
* [SourceLink](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink) support

^1 - [Polyfill](https://github.com/SimonCropp/Polyfill) is a dependency only required for .NET Standard 2.0 and 2.1 users. [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode) is a dependency only required for .NET Standard 2.0 users.

^2 - The Specialization library is distributed separately.

## Why use CliRunner over [CliWrap](https://github.com/Tyrrrz/CliWrap/)?
* Greater separation of concerns with the Command class - Command Building, Command Running, andCommand Pipe handling are moved to separate classes.
* Supports Dependency Injection
* Classes and code follow the Single Responsibility Principle
* No hidden or additional licensing terms are required beyond the source code license.
* No imported C code - This library is entirely written in C#.
* No lock in regarding Piping support - Use .NET's StreamWriter and StreamReader classes as inputs and outputs respectively.
* Uses .NET's built in ``Process`` type.



## Supported Platforms 
This can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

| Operating System | Support Status                     | Notes                                                                                       |
|------------------|------------------------------------|---------------------------------------------------------------------------------------------|
| Windows          | Fully Supported :white_check_mark: |                                                                                             |
| macOS            | Fully Supported :white_check_mark: |                                                                                             |
| Mac Catalyst     | Untested Platform :warning:        | Support for this platform has not been tested but should theoretically work.                |
| Linux            | Fully Supported :white_check_mark: |                                                                                             |
| FreeBSD          | Fully Supported :white_check_mark: |                                                                                             |
| Android          | Untested Platform :warning:        | Support for this platform has not been tested but should theoretically work.                |
| IOS              | Not Supported :x:                  | Not supported due to ``Process.Start()`` not supporting IOS. ^3                             | 
| tvOS             | Not Supported :x:                  | Not supported due to ``Process.Start()`` not supporting tvOS ^3                             |
| watchOS          | Not Supported :x:                  | Not supported due to ``Process.Start()`` not supporting watchOS ^4                          |
| Browser          | Not Supported and Not Planned :x:  | Not supported due to not being a valid target Platform for executing programs or processes. |

^3 - See the [Process class documentation](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-9.0#system-diagnostics-process-start) for more info.

^4 - Lack of watchOS support is implied by lack of IOS support since [watchOS is based on IOS](https://en.wikipedia.org/wiki/WatchOS).


**Note:** This library has not been tested on Android or Tizen!

## Installation
* [Nuget](https://nuget.org/packages/) or ``dotnet add package CliRunner``

## Usage

### Main Use Cases
One of the main use cases for CliRunner is intended to be executing programs programatically, but other valid use cases also exist such as [safer Process Running](#safer-process-running).

#### Executing Commands
CliRunner enables use of a fluent builder style of syntax to easily configure and run Commands.

The following example shows how to configure and build a Command that returns a BufferedProcessResult which contains redirected StandardOutput and StandardError as strings.

```csharp
using CliRunner;
using CliRunner.Builders.Abstractions;
using CliRunner.Builders;

  //Namespace and classs code ommitted for clarity 

  // ServiceProvider and Dependency Injection code ommitted for clarity
  
  ICommandRunner _commandRunner = serviceProvider.GetRequiredService<ICommandRunner>();

  // Fluently configure your Command.
  ICommandBuilder builder = new CommandBuilder("Path/To/Executable")
                            .WithArguments(["arg1", "arg2"])
                            .WithWorkingDirectory("/Path/To/Directory");
  
  // Build it as a Command object when you're ready to use it.
  Command command = builder.Build();
  
  // Execute the command through CommandRunner and get the results.
BufferedProcessResult result = await _commandRunner.ExecuteBufferedAsync(command);
```

#### Safe(r) Process Running
CliRunner also offers safer abstractions around Process Running to avoid accidentally not disposing of Processes after they are executed.

If directly executing the process and receiving a ``ProcessResult`` or ``BufferedProcessResult`` object is desirable you should use ``IProcessRunner`` as a service.

If you don't wish to immediately dispose of the Process after executing it, but plan to dispose of it later then ``IProcessRunnerUtility`` provides more flexibility.

##### ``IProcessRunner``
**Note**: ``IProcessRunner`` and it's implementation class's execution methods do require a ``ProcessResultValidation`` argument to be passed, that configures whether validation should be performed on the Process' exit code.
A default value for the parameter is intentionally not provided, as it is up to the user to decide whether they require exit code validation.

This example shows how it might be used:
```csharp
using CliRunner.Runners;
using CliRunner.Runners.Abstractions;
// Using namespaces for Dependency Injection code ommitted for clarity

//Namespace and classs code ommitted for clarity 

      // ServiceProvider and Dependency Injection code ommitted for clarity

    IProcessRunner _processRunner = serviceProvider.GetRequiredService<IProcessRunner>();
    

    ProcessResult result = await _processRunner.ExecuteProcessAsync(process, ProcessResultValidation.None);
```

Asynchronous methods in ``IProcessRunner`` do provide an optional CancellationToken parameter.

Synchronous methods are available in ``IProcessRunner`` but should be used as a last resort, in situations where using async and await are not possible. 

##### ``IProcessRunnerUtility``
The naming of ``IProcessRunnerUtility`` is deliberately similar to ``IProcessRunner`` as it is the utility interface (and corresponding implementing class) that ``IProcessRunner`` uses behind the scenes for functionality.

Usage of ``IProcessRunnerUtility`` is most appropriate when greater flexibility is required than what ``IProcessRunner`` provides.

For instance, you can keep a Process object alive for as long as needed, and then dispose of it later.

```csharp
using CliRunner.Runners;
using CliRunner.Runners.Abstractions;
// Using namespaces for Dependency Injection code ommitted for clarity

//Namespace and classs code ommitted for clarity 

      // ServiceProvider and Dependency Injection code ommitted for clarity

    IProcessRunnerUtility _processRunnerUtility = serviceProvider.GetRequiredService<IProcessRunnerUtility>();
    
    // Result Validation and Cancellation token are optional parameters.
    int exitCode = await _processRunnerUtility.ExecuteAsync(process);
    
    // Getting the result afterwards is done manually.
    ProcessResult = await _processRunnerUtility.GetResultAsync(process);
    
    // Code continuing to use process object ommitted.
    
    
    // Dispose of Process when no longer needed.
    _processRunnerUtility.DisposeOfProcess(process);
```

Some synchronous methods are available in ``IProcessRunnerUtility`` but should be used as a last resort, in situations where using async and await are not possible.

## License
CliRunner is licensed under the MPL 2.0 license. If you modify any of CliRunner's files then the modified files must be licensed under the MPL 2.0 .

If you use CliRunner in your project please make an exact copy of the contents of CliRunner's LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.

## Acknowledgements
This project would like to thank the following projects for their work:
* [CliWrap](https://github.com/Tyrrrz/CliWrap/) for inspiring this project
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support
* [Microsoft.Bcl.HashCode](https://github.com/dotnet/maintenance-packages) for providing a backport of the HashCode class and static methods to .NET Standard 2.0

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/CliRunner/blob/main/THIRD_PARTY_NOTICES.txt).