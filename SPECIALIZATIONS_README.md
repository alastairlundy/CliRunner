# CliRunner.Specializations
This readme covers the **CliRunner Specializations** library. Looking for the [CliRunner Readme](https://github.com/alastairlundy/CliRunner/blob/main/README.md)?

[![NuGet](https://img.shields.io/nuget/v/CliRunner.Specializations.svg)](https://www.nuget.org/packages/CliRunner.Specializations/)
[![NuGet](https://img.shields.io/nuget/dt/CliRunner.Specializations.svg)](https://www.nuget.org/packages/CliRunner.Specializations/)

## Usage
CliRunner.Specializations comes with 3 specializations as of 0.8.0: 
- [CmdCommand](#cmdcommand) - An easier way to execute processes and commands through cmd.exe (Only supported on Windows)
- [ClassicPowershellCommand](#classicpower-shellcommand) - An easier way to execute processes and commands through Windows Powershell (Only supported on Windows)
- [PowershellCommand](#powershellcommand) - An easier way to execute processes and commands through the modern Cross-Platform open source Powershell (Powershell is not installed by CliRunner and is expected to be installed if you plan to use it.)

All Command specialization classes come with an already configured TargetFilePath that points to the relevant executable.

### CmdCommand
The CmdCommand's TargetFilePath points to Windows' copy of cmd.exe .

```csharp
using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using CliRunner.Specializations.Configurations;
using CliRunner.Specializations;

  /// Initialize CommandRunner with Dependency Injection.
  ServiceCollection services = new ServiceCollection();
  services.UseCliRunner();

  ServiceProvider sp = services.Build();
  ICommandRunner _commandRunner = sp.GetService<ICommandRunner>();

  //Build your command fluently
  ICommandBuilder builder = new CommandBuilder(
          new CmdCommandConfiguration("Your arguments go here"))
                .WithWorkingDirectory(Environment.SystemDirectory);
  
  Command command = builder.Build();
  
  var result = await command.ExecuteBufferedAsync(command);
```

If the result of the command being run is not of concern you can call ``ExecuteAsync()`` instead of ``ExecuteBufferedAsync()`` and ignore the returned CommandResult like so:
```csharp
using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using CliRunner.Specializations.Configurations;
using CliRunner.Specializations;

    // ServiceProvider and Dependency Injection code ommitted for clarity

  ICommandRunner _commandRunner = serviceProvider.GetRequiredService<ICommandRunner>();

  //Build your command fluently
  ICommandBuilder builder = new CommandBuilder(
          new CmdCommandConfiguration("Your arguments go here"))
                .WithWorkingDirectory(Environment.SystemDirectory);
  
  Command command = builder.Build();
  
  var result = await command.ExecuteAsync(command);
```

### ClassicPowershellCommand
The ClassicPowershellCommand is a specialized Command class with an already configured TargetFilePath that points to Windows' copy of powershell.exe .

```csharp
using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using CliRunner.Specializations;

    // ServiceProvider and Dependency Injection code ommitted for clarity

  ICommandRunner _commandRunner = serviceProvider.GetRequiredService<ICommandRunner>();

   //Build your command fluently
  ICommandBuilder builder = new CommandBuilder(
          new ClassicPowershellCommandConfiguration("Your arguments go here"))
                .WithWorkingDirectory(Environment.SystemDirectory);
  
  Command command = builder.Build();
  
 var result = await _commandRunner.ExecuteBufferedAsync(command);
```

### PowershellCommand
The PowershellCommand's TargetFilePath points to the installed copy of cross-platform Powershell if it is installed.

```csharp
using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using CliRunner.Specializations;

    // ServiceProvider and Dependency Injection code ommitted for clarity

  ICommandRunner _commandRunner = serviceProvider.GetRequiredService<ICommandRunner>();

   //Build your command fluently
  ICommandBuilder builder = new CommandBuilder(
          new PowershellCommandConfiguration("Your arguments go here"))
                .WithWorkingDirectory(Environment.SystemDirectory);
  
  Command command = builder.Build();
  
 var result = await _commandRunner.ExecuteBufferedAsync(command);
```

## Licensing
CliRunner and CliRunner Specializations are licensed under the MPL 2.0 license. If you modify any of CliRunner's or CliRunner.Specialization's files then the modified files must be licensed under the MPL 2.0 .

If you use CliRunner or CliRunner.Specializations in your project please make an exact copy of the contents of CliRunner's LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.
