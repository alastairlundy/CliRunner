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
    public ICommandBuilder WithArguments(IEnumerable<string> arguments)
    {
       Command command = new(_command.TargetFilePath,
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
            _command.UseShellExecution);
       
       return new CommandBuilder(command);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="escapeArguments"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithArguments(string arguments)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetFilePath"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithTargetFile(string targetFilePath)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="environmentVariables"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
    {
        EnvironmentVariablesBuilder environmentVariablesBuilder = new EnvironmentVariablesBuilder()
            .Set(_command.EnvironmentVariables);

        configure(environmentVariablesBuilder);

        return WithEnvironmentVariables(environmentVariablesBuilder.Build());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithAdministratorPrivileges(bool runAsAdministrator)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="workingDirectoryPath"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithWorkingDirectory(string workingDirectoryPath)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithCredentials(UserCredentials credentials)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    [Pure]
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
    /// 
    /// </summary>
    /// <param name="validation"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithValidation(CommandResultValidation validation)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithStandardInputPipe(StreamWriter source)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithStandardOutputPipe(StreamReader target)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithStandardErrorPipe(StreamReader target)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processorAffinity"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("watchos")]
#endif
    [Pure]
    public ICommandBuilder WithProcessorAffinity(IntPtr processorAffinity)
    {
        return new CommandBuilder(
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="useShellExecution"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithShellExecution(bool useShellExecution)
    {
        return new CommandBuilder(
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
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enableWindowCreation"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithWindowCreation(bool enableWindowCreation)
    {
       Command command = new(_command.TargetFilePath,
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
           _command.UseShellExecution);

       return new CommandBuilder(command);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="standardInputEncoding"></param>
    /// <param name="standardOutputEncoding"></param>
    /// <param name="standardErrorEncoding"></param>
    /// <returns></returns>
    [Pure]
    public ICommandBuilder WithEncoding(Encoding standardInputEncoding = default,
        Encoding standardOutputEncoding = default,
        Encoding standardErrorEncoding = default)
    {
        Command command = new (_command.TargetFilePath,
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
            _command.UseShellExecution);
        
        return new CommandBuilder(command);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Pure]
    public Command Build() => _command;
}