/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

using CliRunner.Abstractions;
using CliRunner.Builders.Abstractions;
using CliRunner.Internal.Localizations;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
// ReSharper disable RedundantBoolCompare
#endif

namespace CliRunner.Builders;

/// <summary>
/// A class to build Commands and Command Configurations with a Fluent configuration interface. 
/// </summary>
[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class CommandBuilder : ICommandBuilder
{
    private readonly ICommandConfiguration _commandConfiguration;

    /// <summary>
    /// Instantiates the CommandBuilder with a specified target file path of an ICommandConfiguration.
    /// </summary>
    /// <param name="targetFilePath">The target file path of a Command to be executed.</param>
    public CommandBuilder(string targetFilePath)
    {
        _commandConfiguration = new Command(targetFilePath, processResourcePolicy: ProcessResourcePolicy.Default);
    }

    /// <summary>
    /// Instantiates the CommandBuilder with a specified ICommandConfiguration configuration
    /// </summary>
    /// <param name="commandConfiguration">The configuration to be used when building the Command.</param>
    public CommandBuilder(ICommandConfiguration commandConfiguration)
    {
        _commandConfiguration = commandConfiguration;
    }
    
    protected CommandBuilder(Command command)
    {
        _commandConfiguration = command;
    }
    
    /// <summary>
    /// Sets the arguments to pass to the executable.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the Command.</param>
    /// <returns>The updated ICommandBuilder object with the specified arguments.</returns>
    [Pure]
    public ICommandBuilder WithArguments(IEnumerable<string> arguments) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                string.Join(" ", arguments),
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the arguments to pass to the executable.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <param name="escapeArguments">Whether to escape the arguments if escape characters are detected.</param>
    /// <returns>The new ICommandBuilder object with the specified arguments.</returns>
    [Pure]
    public ICommandBuilder WithArguments(IEnumerable<string> arguments, bool escapeArguments)
    {
        IArgumentsBuilder argumentsBuilder = new ArgumentsBuilder();
        argumentsBuilder.Add(arguments, escapeArguments);
        
        string args = argumentsBuilder.ToString();

        return new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
            args,
            _commandConfiguration.WorkingDirectoryPath,
            _commandConfiguration.RequiresAdministrator,
            _commandConfiguration.EnvironmentVariables,
            _commandConfiguration.Credential,
            _commandConfiguration.ResultValidation,
            _commandConfiguration.StandardInput,
            _commandConfiguration.StandardOutput,
            _commandConfiguration.StandardError,
            _commandConfiguration.StandardInputEncoding,
            _commandConfiguration.StandardOutputEncoding,
            _commandConfiguration.StandardErrorEncoding,
            _commandConfiguration.ResourcePolicy,
            _commandConfiguration.WindowCreation,
            _commandConfiguration.UseShellExecution));
    }
    
    /// <summary>
    /// Sets the arguments to pass to the executable.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <returns>The new ICommandBuilder object with the specified arguments.</returns>
    [Pure]
    public ICommandBuilder WithArguments(string arguments) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the Target File Path of the Command Executable.
    /// </summary>
    /// <param name="targetFilePath">The target file path of the Command.</param>
    /// <returns>The Command with the updated Target File Path.</returns>
    [Pure]
    public ICommandBuilder WithTargetFile(string targetFilePath) =>
        new CommandBuilder(
            new Command(targetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));
    
    /// <summary>
    /// Sets the environment variables to be configured.
    /// </summary>
    /// <param name="environmentVariables">The environment variables to be configured.</param>
    /// <returns>The new CommandBuilder with the specified environment variables.</returns>
    [Pure]
    public ICommandBuilder WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                environmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the environment variables for the Command to be executed.
    /// </summary>
    /// <param name="configure">The environment variables to be configured</param>
    /// <returns>The new CommandBuilder with the specified environment variables.</returns>
    [Pure]
    public ICommandBuilder WithEnvironmentVariables(Action<IEnvironmentVariablesBuilder> configure)
    {
        IEnvironmentVariablesBuilder environmentVariablesBuilder = new EnvironmentVariablesBuilder()
            .Set(_commandConfiguration.EnvironmentVariables);

        configure(environmentVariablesBuilder);

        return WithEnvironmentVariables(environmentVariablesBuilder.Build());
    }

    /// <summary>
    /// Sets whether to execute the Command with Administrator Privileges.
    /// </summary>
    /// <param name="runAsAdministrator">Whether to execute the Command with Administrator Privileges.</param>
    /// <returns>The new CommandBuilder with the specified Administrator Privileges settings.</returns>
    [Pure]
    public ICommandBuilder WithAdministratorPrivileges(bool runAsAdministrator) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                runAsAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the working directory to be used for the Command.
    /// </summary>
    /// <param name="workingDirectoryPath">The working directory to be used.</param>
    /// <returns>The new CommandBuilder with the specified working directory.</returns>
    [Pure]
    public ICommandBuilder WithWorkingDirectory(string workingDirectoryPath) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                workingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the specified Credentials to be used.
    /// </summary>
    /// <param name="credentials">The credentials to be used.</param>
    /// <returns>The new CommandBuilder with the specified Credentials.</returns>
    /// <remarks>Credentials are only supported with the Process class on Windows. This is a limitation of .NET's Process class.</remarks>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("android")]
#endif
    [Pure]
    public ICommandBuilder WithUserCredential(UserCredential credentials) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                credentials,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the credentials for the Command to be executed.
    /// </summary>
    /// <param name="configure">The CredentialsBuilder configuration.</param>
    /// <returns>The new CommandBuilder with the specified Credentials.</returns>
    /// <remarks>Credentials are only supported with the Process class on Windows. This is a limitation of .NET's Process class.</remarks>
    [Pure]
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("android")]
#endif
    public ICommandBuilder WithUserCredential(Action<IUserCredentialBuilder> configure)
    {
        IUserCredentialBuilder credentialBuilder = new UserCredentialBuilder()
            .SetDomain(_commandConfiguration.Credential.Domain!)
            .SetPassword(_commandConfiguration.Credential.Password!)
            .SetUsername(_commandConfiguration.Credential.UserName!);

        configure(credentialBuilder);

        return WithUserCredential(credentialBuilder.Build());
    }

    /// <summary>
    /// Sets the Result Validation whether to throw an exception or not if the Command does not execute successfully.
    /// </summary>
    /// <param name="validation">The result validation behaviour to be used.</param>
    /// <returns>The new CommandBuilder object with the configured Result Validation behaviour.</returns>
    [Pure]
    public ICommandBuilder WithValidation(ProcessResultValidation validation) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                validation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Sets the Standard Input Pipe source.
    /// </summary>
    /// <param name="source">The source to use for the Standard Input pipe.</param>
    /// <returns>The new CommandBuilder with the specified Standard Input pipe source.</returns>
    /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardinput"/>
    [Pure]
    public ICommandBuilder WithStandardInputPipe(StreamWriter source)
    {
        if (_commandConfiguration.UseShellExecution == true && source != StreamWriter.Null)
        {
            throw new ArgumentException(Resources.CommandBuilder_ArgumentConflict_ShellExecution_Input);
        }
        else
        {
            return new CommandBuilder(
                new Command(
                    _commandConfiguration.TargetFilePath,
                    _commandConfiguration.Arguments,
                    _commandConfiguration.WorkingDirectoryPath,
                    _commandConfiguration.RequiresAdministrator,
                    _commandConfiguration.EnvironmentVariables,
                    _commandConfiguration.Credential,
                    _commandConfiguration.ResultValidation,
                    source,
                    _commandConfiguration.StandardOutput,
                    _commandConfiguration.StandardError,
                    _commandConfiguration.StandardInputEncoding,
                    _commandConfiguration.StandardOutputEncoding,
                    _commandConfiguration.StandardErrorEncoding,
                    _commandConfiguration.ResourcePolicy,
                    _commandConfiguration.WindowCreation,
                    _commandConfiguration.UseShellExecution));
        }
    }

    /// <summary>
    /// Sets the Standard Output Pipe target.
    /// </summary>
    /// <param name="target">The target to send the Standard Output to.</param>
    /// <returns>The new CommandBuilder with the specified Standard Output Pipe Target.</returns>
    /// <remarks>Using Shell Execution whilst also Redirecting Standard Output will throw an Exception. This is a known issue with the System Process class.</remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput"/>
    [Pure]
    public ICommandBuilder WithStandardOutputPipe(StreamReader target)
    {
        if (_commandConfiguration.UseShellExecution == true && target != StreamReader.Null)
        {
            throw new ArgumentException(Resources.CommandBuilder_ArgumentConflict_ShellExecution_Output);
        }
        else
        {
            return new CommandBuilder(
                new Command(
                    _commandConfiguration.TargetFilePath,
                    _commandConfiguration.Arguments,
                    _commandConfiguration.WorkingDirectoryPath,
                    _commandConfiguration.RequiresAdministrator,
                    _commandConfiguration.EnvironmentVariables,
                    _commandConfiguration.Credential,
                    _commandConfiguration.ResultValidation,
                    _commandConfiguration.StandardInput,
                    target,
                    _commandConfiguration.StandardError,
                    _commandConfiguration.StandardInputEncoding,
                    _commandConfiguration.StandardOutputEncoding,
                    _commandConfiguration.StandardErrorEncoding,
                    _commandConfiguration.ResourcePolicy,
                    _commandConfiguration.WindowCreation,
                    _commandConfiguration.UseShellExecution));
        }
    }

    /// <summary>
    /// Sets the Standard Error Pipe target.
    /// </summary>
    /// <param name="target">The target to send the Standard Error to.</param>
    /// <returns>The new CommandBuilder with the specified Standard Error Pipe Target.</returns>
    /// <remarks>Using Shell Execution whilst also Redirecting Standard Error will throw an Exception. This is a known issue with the System Process class.</remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror"/>
    [Pure]
    public ICommandBuilder WithStandardErrorPipe(StreamReader target)
    {
        if (_commandConfiguration.UseShellExecution == true && target != StreamReader.Null)
        {
            throw new ArgumentException(Resources.CommandBuilder_ArgumentConflict_ShellExecution_Error);
        }
        else
        {
           return new CommandBuilder(
                new Command(
                    _commandConfiguration.TargetFilePath,
                    _commandConfiguration.Arguments,
                    _commandConfiguration.WorkingDirectoryPath,
                    _commandConfiguration.RequiresAdministrator,
                    _commandConfiguration.EnvironmentVariables,
                    _commandConfiguration.Credential,
                    _commandConfiguration.ResultValidation,
                    _commandConfiguration.StandardInput,
                    _commandConfiguration.StandardOutput,
                    target,
                    _commandConfiguration.StandardInputEncoding,
                    _commandConfiguration.StandardOutputEncoding,
                    _commandConfiguration.StandardErrorEncoding,
                    _commandConfiguration.ResourcePolicy,
                    _commandConfiguration.WindowCreation,
                    _commandConfiguration.UseShellExecution));
        }
    }

    /// <summary>
    /// Sets the Process Resource Policy to be used for this command.
    /// </summary>
    /// <param name="processResourcePolicy">The process resource policy to use.</param>
    /// <returns>The new CommandBuilder with the specified Process Resource Policy.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("android")]
#endif
    [Pure]
    public ICommandBuilder WithProcessResourcePolicy(ProcessResourcePolicy processResourcePolicy) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                processResourcePolicy,
                _commandConfiguration.WindowCreation,
                _commandConfiguration.UseShellExecution));
    

    /// <summary>
    /// Enables or disables command execution via Shell Execution.
    /// </summary>
    /// <param name="useShellExecution">Whether to enable or disable shell execution.</param>
    /// <returns>The new CommandBuilder with the specified shell execution behaviour.</returns>
    /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror"/>
    [Pure]
    public ICommandBuilder WithShellExecution(bool useShellExecution) =>
        new CommandBuilder(
            new Command(_commandConfiguration.TargetFilePath,
                _commandConfiguration.Arguments,
                _commandConfiguration.WorkingDirectoryPath,
                _commandConfiguration.RequiresAdministrator,
                _commandConfiguration.EnvironmentVariables,
                _commandConfiguration.Credential,
                _commandConfiguration.ResultValidation,
                _commandConfiguration.StandardInput,
                _commandConfiguration.StandardOutput,
                _commandConfiguration.StandardError,
                _commandConfiguration.StandardInputEncoding,
                _commandConfiguration.StandardOutputEncoding,
                _commandConfiguration.StandardErrorEncoding,
                _commandConfiguration.ResourcePolicy,
                _commandConfiguration.WindowCreation,
                useShellExecution));

    /// <summary>
    /// Enables or disables Window creation for the wrapped executable.
    /// </summary>
    /// <param name="enableWindowCreation">Whether to enable or disable window creation for the wrapped executable.</param>
    /// <returns>The new CommandBuilder with the specified window creation behaviour.</returns>
    [Pure]
    public ICommandBuilder WithWindowCreation(bool enableWindowCreation) =>
        new CommandBuilder(new Command(_commandConfiguration.TargetFilePath,
           _commandConfiguration.Arguments,
           _commandConfiguration.WorkingDirectoryPath,
           _commandConfiguration.RequiresAdministrator,
           _commandConfiguration.EnvironmentVariables,
           _commandConfiguration.Credential,
           _commandConfiguration.ResultValidation,
           _commandConfiguration.StandardInput,
           _commandConfiguration.StandardOutput,
           _commandConfiguration.StandardError,
           _commandConfiguration.StandardInputEncoding,
           _commandConfiguration.StandardOutputEncoding,
           _commandConfiguration.StandardErrorEncoding,
           _commandConfiguration.ResourcePolicy,
            enableWindowCreation,
           _commandConfiguration.UseShellExecution));
    
    
    /// <summary>
    /// Sets the Encoding types to be used for Standard Input, Output, and Error.
    /// </summary>
    /// <param name="standardInputEncoding">The encoding type to be used for the Standard Input.</param>
    /// <param name="standardOutputEncoding">The encoding type to be used for the Standard Output.</param>
    /// <param name="standardErrorEncoding">The encoding type to be used for the Standard Error.</param>
    /// <returns>The new ICommandBuilder with the specified Pipe Encoding types.</returns>
    [Pure]
    public ICommandBuilder WithEncoding(Encoding standardInputEncoding = null,
        Encoding standardOutputEncoding = null,
        Encoding standardErrorEncoding = null) =>
        new CommandBuilder(new Command(_commandConfiguration.TargetFilePath,
            _commandConfiguration.Arguments,
            _commandConfiguration.WorkingDirectoryPath,
            _commandConfiguration.RequiresAdministrator,
            _commandConfiguration.EnvironmentVariables,
            _commandConfiguration.Credential,
            _commandConfiguration.ResultValidation,
            _commandConfiguration.StandardInput,
            _commandConfiguration.StandardOutput,
            _commandConfiguration.StandardError,
            standardInputEncoding,
            standardOutputEncoding,
            standardErrorEncoding,
            _commandConfiguration.ResourcePolicy,
            _commandConfiguration.WindowCreation,
            _commandConfiguration.UseShellExecution));

    /// <summary>
    /// Builds the Command with the configured parameters.
    /// </summary>
    /// <returns>The newly configured Command.</returns>
    [Pure]
    public Command Build() => new Command(_commandConfiguration);

    /// <summary>
    /// Builds the ICommandConfiguration with the configured parameters.
    /// </summary>
    /// <returns>The new ICommandConfiguration.</returns>
    public ICommandConfiguration BuildConfiguration() => _commandConfiguration;
}