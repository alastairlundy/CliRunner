# CliRunner
CliRunner is a library for interacting with Command Line Interfaces and wrapping around executables.

<img src="https://github.com/alastairlundy/CliRunner/blob/main/.assets/icon.png" width="192" height="192" alt="CliRunner Logo">

[![NuGet](https://img.shields.io/nuget/v/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/) 
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.svg)](https://www.nuget.org/packages/CliRunner/)

## Table of Contents
* [Features](#features)
* [Why CliRunner?](#why-use-clirunner-over-cliwrap)
* [Installing CliRunner](#how-to-install-and-use-clirunner)
    * [Installing CliRunner](#installing-clirunner)
    * [Supported Platforms](#supported-platforms)
* [CLiRunner Examples](#using-clirunner--examples)
      * [Executing Commands](#executing-commands)
      * [Running Processes Safely](#safe-process-running)
* [Contributing to CliRunner](#how-to-contribute-to-clirunner)
* [Roadmap](#clirunners-roadmap)
* [License](#license)
  * [CliRunner Assets](#clirunner-assets)
* [Acknowledgements](#acknowledgements)

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

## How to install and use CliRunner
CliRunner is available on [Nuget](https://nuget.org).

These are the CliRunner projects:
* CliRunner - The main CliRunner package.
* [CliRunner.Extensions.DependencyInjection](CliRunnerLibrary/CliRunner.Extensions.DependencyInjection/README.md)
* [CliRunner.Specializations](SPECIALIZATIONS_README.md)

### Installing CliRunner
CliRunner's packages can be installed via the .NET SDK CLI, Nuget via your IDE or code editor's package interface, or via the Nuget website.

| Package Name                             | Nuget Link                                                                                                            | .NET SDK CLI command                                            |
|------------------------------------------|-----------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------|
| CliRunner                                | [CliRunner Nuget](https://nuget.org/packages/CliRunner)                                                               | ``dotnet add package CliRunner``                                |
| CliRunner.Extensions.DependencyInjection | [CliRunner.Extensions.DependencyInjection Nuget](https://nuget.org/packages/CliRunner.Extensions.DependencyInjection) | ``dotnet add package CliRunner.Extensions.DependencyInjection`` |
| CliRunner.Specializations                | [CliRunner.Specializations Nuget](https://nuget.org/packages/CliRunner.Specializations)                               | ``dotnet add package CliRunner.Specializations``                |


### Supported Platforms
CliRunner can be added to any .NET Standard 2.0, .NET Standard 2.1, .NET 8, or .NET 9 supported project.

The following table details which target platforms are supported for executing commands via CliRunner. 

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

## Using CliRunner / Examples
One of the main use cases for CliRunner is intended to be executing programs programatically, but other valid use cases also exist such as [safer Process Running](#safer-process-running).

### Executing Commands
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

### Safe Process Running
CliRunner also offers safe abstractions around Process Running to avoid accidentally not disposing of Processes after they are executed.

If directly executing the process and receiving a ``ProcessResult`` or ``BufferedProcessResult`` object is desirable you should use ``IProcessRunner`` as a service.

If you don't wish to immediately dispose of the Process after executing it, but plan to dispose of it later then ``IProcessRunnerUtility`` provides more flexibility.

#### ``IProcessRunner``
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

## How to Build CliRunner's code

### Requirements
CliRunner requires the latest .NET release SDK to be installed to target all supported TFM (Target Framework Moniker) build targets.

Currently, the required .NET SDK is .NET 9. 

The current build targets include: 
* .NET 8
* .NET 9
* .NET Standard 2.0
* .NET Standard 2.1

Any version of the .NET 9 SDK can be used, but using the latest version is preferred.

### Versioning new releases
CliRunner aims to follow Semantic versioning with ```[Major].[Minor].[Build]``` for most circumstances and an optional ``.[Revision]`` when only a configuration change is made, or a new build of a preview release is made.

#### Pre-releases
Pre-release versions should have a suffix of -alpha, -beta, -rc, or -preview followed by a ``.`` and what pre-release version number they are. The number should be incremented by 1 after each release unless it only contains a configuration change, or another packaging, or build change. An example pre-release version may look like 1.1.0-alpha.2 , this version string would indicate it is the 2nd alpha pre-release version of 1.1.0 .

#### Stable Releases
Stable versions should follow semantic versioning and should only increment the Revision number if a release only contains configuration or build packaging changes, with no change in functionality, features, or even bug or security fixes.

Releases that only implement bug fixes should see the Build version incremented.

Releases that add new non-breaking changes should increment the Minor version. Minor breaking changes may be permitted in Minor version releases where doing so is necessary to maintain compatibility with an existing supported platform, or an existing piece of code that requires a breaking change to continue to function as intended.

Releases that add major breaking changes or significantly affect the API should increment the Major version. Major version releases should not be released with excessive frequency and should be released when there is a genuine need for the API to change significantly for the improvement of the project.

### Building for Testing
You can build for testing by building the desired project within your IDE or VS Code, or manually by entering the following command: ``dotnet build -c Debug``.

If you encounter any bugs or issues, try running the ``CliRunner.Tests`` project and setting breakpoints in the affected CliRunner project's code where appropriate. Failing that, please [report the issue](https://github.com/alastairlundy/CliRunner/issues/new/) if one doesn't already exist for the bug(s).

### Building for Release
Before building a release build, ensure you apply the relevant changes to the relevant ``.csproj`` file corresponding to the package you are trying to build:
* Update the Package Version variable 
* Update the project file's Changelog
* Remove/replace the CliRunner icon if distributing a non-official release build to a wider audience. See [CliRunner Assets](#clirunner-assets) for more details.

You should ensure the project builds under debug settings before producing a release build.

#### Producing Release Builds
To manually build a project for release, enter ``dotnet build -c Release /p:ContinuousIntegrationBuild=true`` for a release with [SourceLink](https://github.com/dotnet/sourcelink) enabled or just ``dotnet build -c Release`` for a build without sourcelink.

Builds should generally always include Source Link and symbol packages if intended for wider distribution.

**NOTES**: 
* ``CliRunner.Specializations`` and ``CliRunner.Extensions.DependencyInjection`` both take a dependency on the CliRunner base package from Nuget - For the respective libraries to use a newer CliRunner version, that version must be published on Nuget.

## How to Contribute to CliRunner
Thank you in advance for considering contributing to CliRunner.

Please see the [CONTRIBUTING.md file](CONTRIBUTING.md) for code and localization contributions.

If you want to file a bug report or suggest a potential feature to add, please check out the [GitHub issues page](https://github.com/alastairlundy/CliRunner/issues/) to see if a similar or identical issue is already open.
If there is not already a relevant issue filed, please [file one here](https://github.com/alastairlundy/CliRunner/issues/new) and follow the respective guidance from the appropriate issue template.

Thanks.

## CliRunner's Roadmap
CliRunner aims to make working with Commands and external processes easier.

Whilst an initial set of features are available in version 1, there is room for more features, and for modifications of existing features in future updates.

That being said, all stable releases from 1.0 onwards must be stable and should not contain regressions.

Future updates should aim focus on one or more of the following:
* Improved ease of use
* Improved stability 
* New features
* Enhancing existing features

## License
CliRunner is licensed under the MPL 2.0 license. If you modify any of CliRunner's files then the modified files must be licensed under the MPL 2.0 .

If you use CliRunner in your project please make an exact copy of the contents of CliRunner's LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.

### CliRunner Assets
CliRunner's Icon is NOT licensed under the MPL 2.0 license and are licensed under Copyright with all rights reserved to me (Alastair Lundy).

If you fork CliRunner and re-distribute it, please replace the usage of the icon unless you have prior written agreements from me. 

## Acknowledgements

### Projects
This project would like to thank the following projects for their work:
* [CliWrap](https://github.com/Tyrrrz/CliWrap/) for inspiring this project
* [Polyfill](https://github.com/SimonCropp/Polyfill) for simplifying .NET Standard 2.0 & 2.1 support
* [Microsoft.Bcl.HashCode](https://github.com/dotnet/maintenance-packages) for providing a backport of the HashCode class and static methods to .NET Standard 2.0

For more information, please see the [THIRD_PARTY_NOTICES file](https://github.com/alastairlundy/CliRunner/blob/main/THIRD_PARTY_NOTICES.txt).
