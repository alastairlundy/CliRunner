# CliRunner.Specializations
This readme covers the **CliRunner Specializations** library. Looking for the [CliRunner Readme](https://github.com/alastairlundy/CliRunner/blob/main/README.md)?

## Usage
CliRunner.Specializations comes with 3 specializations as of 0.3.0: 
- [CmdCommand](#cmd-command) - An easier way to execute processes and commands through cmd.exe (Only supported on Windows)
- [ClassicPowershellCommand](#classic-power-shell-command) - An easier way to execute processes and commands through Windows Powershell (Only supported on Windows)
- [PowershellCommand](#power-shell-command) - An easier way to execute processes and commands through the modern Cross-Platform open source Powershell (Powershell is not installed by CliRunner and is expected to be installed if you plan to use it.)

All Command specialization classes come with an already configured TargetFilePath that points to the relevant executable.

### CmdCommand
The CmdCommand's TargetFilePath points to Windows' copy of cmd.exe .

Usage is identical to using ``Cli.Run`` except that the entrypoint is ``CmdCommand.Create()`` - This is a static method that instantiates the Command for use with the usual Command builder methods that are also supported with Command Specializations

```csharp
  var result = await CmdCommand.Create()
                .WithArguments("Your arguments go here")
                .WithWorkingDirectory(Environment.SystemDirectory)
                .ExecuteBufferedAsync();
```

If the result of the command being run is not of concern you can call ``ExecuteAsync()`` instead of ``ExecuteBufferedAsync()`` and ignore the returned CommandResult like so:
```csharp
   await CmdCommand.Create()
                .WithArguments("Your arguments go here")
                .WithWorkingDirectory(Environment.SystemDirectory)
                .ExecuteAsync();
```

### ClassicPowershellCommand
The ClassicPowershellCommand is a specialized Command class with an already configured TargetFilePath that points to Windows' copy of powershell.exe .

Usage is identical to using ``Cli.Run`` except that the entrypoint is ``ClassicPowershellCommand.Create()`` - This is a static method that instantiates the Command for use with the usual Command builder methods that are also supported with Command Specializations

```csharp
 var task = await ClassicPowershellCommand.Create()
                .WithArguments("Your arguments go here")
                .ExecuteBufferedAsync();
```

### PowershellCommand
The PowershellCommand's TargetFilePath points to the installed copy of cross-platform Powershell if it is installed.

Usage is identical to using ``Cli.Run`` except that the entrypoint is ``PowershellCommand.Create()`` - This is a static method that instantiates the Command for use with the usual Command builder methods that are also supported with Command Specializations

```csharp
  var result = await PowershellCommand.Create()
                .WithArguments("Your arguments go here")
                .WithWorkingDirectory(Environment.SystemDirectory)
                .ExecuteBufferedAsync();
```

## Licensing
CliRunner and CliRunner Specializations are licensed under the MPL 2.0 license. If you modify any of CliRunner's or CliRunner.Specialization's files then the modified files must be licensed under the MPL 2.0 .

If you use CliRunner or CliRunner.Specializations in your project please make an exact copy of the contents of CliRunner's LICENSE.txt file available either in your third party licenses txt file or as a separate txt file.
