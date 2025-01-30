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

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace CliRunner.Builders;

/// <summary>
/// A class to build Commands with a Fluent configuration interface. 
/// </summary>
[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class CommandBuilder : ICommandBuilder
{
    private readonly Command _command;

    public CommandBuilder(string targetFilePath)
    {
        _command = new Command(targetFilePath);
    }

    private CommandBuilder(Command command)
    {
        _command = command;
    }
    
    /// <summary>
    /// Sets the arguments to pass to the executable.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the Command.</param>
    /// <returns>The updated ICommandBuilder object with the specified arguments.</returns>
    [Pure]
    public ICommandBuilder WithArguments(IEnumerable<string> arguments) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                string.Join(" ", arguments),
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the arguments to pass to the executable.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <param name="escapeArguments">Whether to escape the arguments if escape characters are detected.</param>
    /// <returns>The new ICommandBuilder object with the specified arguments.</returns>
    [Pure]
    public ICommandBuilder WithArguments(IEnumerable<string> arguments, bool escapeArguments)
    {
        string args = string.Join(" ", arguments);

        if (escapeArguments)
        {
            args = ArgumentsBuilder.Escape(args);
        }

        return new CommandBuilder(
            new Command(_command.TargetFilePath,
            args,
            _command.WorkingDirectoryPath,
            _command.RequiresAdministrator,
            _command.EnvironmentVariables,
            _command.Credentials,
            _command.ResultValidation,
            _command.StandardInput,
            _command.StandardOutput,
            _command.StandardError,
            _command.StandardInputEncoding,
            _command.StandardOutputEncoding,
            _command.StandardErrorEncoding,
            _command.ProcessorAffinity,
            _command.WindowCreation,
            _command.UseShellExecution));
    }
    
    /// <summary>
    /// Sets the arguments to pass to the executable.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <returns>The new ICommandBuilder object with the specified arguments.</returns>
    [Pure]
    public ICommandBuilder WithArguments(string arguments) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the Target File Path of the Command Executable.
    /// </summary>
    /// <param name="targetFilePath">The target file path of the Command.</param>
    /// <returns>The Command with the updated Target File Path.</returns>
    [Pure]
    public ICommandBuilder WithTargetFile(string targetFilePath) =>
        new CommandBuilder(
            new Command(targetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the environment variables to be configured.
    /// </summary>
    /// <param name="environmentVariables">The environment variables to be configured.</param>
    /// <returns>The new CommandBuilder with the specified environment variables.</returns>
    [Pure]
    public ICommandBuilder WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                environmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the environment variables for the Command to be executed.
    /// </summary>
    /// <param name="configure">The environment variables to be configured</param>
    /// <returns>The new CommandBuilder with the specified environment variables.</returns>
    [Pure]
    public ICommandBuilder WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
    {
        EnvironmentVariablesBuilder environmentVariablesBuilder = new EnvironmentVariablesBuilder()
            .Set(_command.EnvironmentVariables);

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
            new Command(_command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                runAsAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the working directory to be used for the Command.
    /// </summary>
    /// <param name="workingDirectoryPath">The working directory to be used.</param>
    /// <returns>The new CommandBuilder with the specified working directory.</returns>
    [Pure]
    public ICommandBuilder WithWorkingDirectory(string workingDirectoryPath) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                _command.Arguments,
                workingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

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
    public ICommandBuilder WithCredentials(UserCredentials credentials) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

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
    public ICommandBuilder WithCredentials(Action<CredentialsBuilder> configure)
    {
        CredentialsBuilder credentialBuilder = new CredentialsBuilder()
            .SetDomain(_command.Credentials.Domain)
            .SetPassword(_command.Credentials.Password)
            .SetUsername(_command.Credentials.UserName);

        configure(credentialBuilder);

        return WithCredentials(credentialBuilder.Build());
    }

    /// <summary>
    /// Sets the Result Validation whether to throw an exception or not if the Command does not execute successfully.
    /// </summary>
    /// <param name="validation">The result validation behaviour to be used.</param>
    /// <returns>The new CommandBuilder object with the configured Result Validation behaviour.</returns>
    [Pure]
    public ICommandBuilder WithValidation(CommandResultValidation validation) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                validation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the Standard Input Pipe source.
    /// </summary>
    /// <param name="source">The source to use for the Standard Input pipe.</param>
    /// <returns>The new CommandBuilder with the specified Standard Input pipe source.</returns>
    /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror"/>
    [Pure]
    public ICommandBuilder WithStandardInputPipe(StreamWriter source) =>
        new CommandBuilder(
            new Command(
                _command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                source,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the Standard Output Pipe target.
    /// </summary>
    /// <param name="target">The target to send the Standard Output to.</param>
    /// <returns>The new CommandBuilder with the specified Standard Output Pipe Target.</returns>
    [Pure]
    public ICommandBuilder WithStandardOutputPipe(StreamReader target) =>
        new CommandBuilder(
            new Command(
                _command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                target,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the Standard Error Pipe target.
    /// </summary>
    /// <param name="target">The target to send the Standard Error to.</param>
    /// <returns>The new CommandBuilder with the specified Standard Error Pipe Target.</returns>
    [Pure]
    public ICommandBuilder WithStandardErrorPipe(StreamReader target) =>
        new CommandBuilder(
            new Command(
                _command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                target,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));

    /// <summary>
    /// Sets the Processor Affinity for this command.
    /// </summary>
    /// <param name="processorAffinity">The processor affinity to use.</param>
    /// <returns>The new CommandBuilder with the specified Processor Affinity.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("android")]
#endif
    [Pure]
    public ICommandBuilder WithProcessorAffinity(IntPtr processorAffinity) =>
        new CommandBuilder(
            new Command(_command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                processorAffinity,
                _command.WindowCreation,
                _command.UseShellExecution));
    

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
            new Command(_command.TargetFilePath,
                _command.Arguments,
                _command.WorkingDirectoryPath,
                _command.RequiresAdministrator,
                _command.EnvironmentVariables,
                _command.Credentials,
                _command.ResultValidation,
                _command.StandardInput,
                _command.StandardOutput,
                _command.StandardError,
                _command.StandardInputEncoding,
                _command.StandardOutputEncoding,
                _command.StandardErrorEncoding,
                _command.ProcessorAffinity,
                _command.WindowCreation,
                useShellExecution));

    /// <summary>
    /// Enables or disables Window creation for the wrapped executable.
    /// </summary>
    /// <param name="enableWindowCreation">Whether to enable or disable window creation for the wrapped executable.</param>
    /// <returns>The new CommandBuilder with the specified window creation behaviour.</returns>
    [Pure]
    public ICommandBuilder WithWindowCreation(bool enableWindowCreation) =>
        new CommandBuilder(new Command(_command.TargetFilePath,
           _command.Arguments,
           _command.WorkingDirectoryPath,
           _command.RequiresAdministrator,
           _command.EnvironmentVariables,
           _command.Credentials,
           _command.ResultValidation,
           _command.StandardInput,
           _command.StandardOutput,
           _command.StandardError,
           _command.StandardInputEncoding,
           _command.StandardOutputEncoding,
           _command.StandardErrorEncoding,
           _command.ProcessorAffinity,
            enableWindowCreation,
           _command.UseShellExecution));
    
    
    /// <summary>
    /// Sets the Encoding types to be used for Standard Input, Output, and Error.
    /// </summary>
    /// <param name="standardInputEncoding">The encoding type to be used for the Standard Input.</param>
    /// <param name="standardOutputEncoding">The encoding type to be used for the Standard Output.</param>
    /// <param name="standardErrorEncoding">The encoding type to be used for the Standard Error.</param>
    /// <returns>The new CommandBuilder with the specified Pipe Encoding types.</returns>
    [Pure]
    public ICommandBuilder WithEncoding(Encoding standardInputEncoding = default,
        Encoding standardOutputEncoding = default,
        Encoding standardErrorEncoding = default) =>
        new CommandBuilder(new Command(_command.TargetFilePath,
            _command.Arguments,
            _command.WorkingDirectoryPath,
            _command.RequiresAdministrator,
            _command.EnvironmentVariables,
            _command.Credentials,
            _command.ResultValidation,
            _command.StandardInput,
            _command.StandardOutput,
            _command.StandardError,
            standardInputEncoding,
            standardOutputEncoding,
            standardErrorEncoding,
            _command.ProcessorAffinity,
            _command.WindowCreation,
            _command.UseShellExecution));

    /// <summary>
    /// Builds the Command with the configured parameters.
    /// </summary>
    /// <returns>The new Command that has been configured.</returns>
    [Pure]
    public Command Build() => _command;
}