﻿/*
    CliInvoke.Extensibility
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using AlastairLundy.Extensions.Processes;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;

// ReSharper disable MemberCanBePrivate.Global

namespace AlastairLundy.CliInvoke.Extensibility.Abstractions.Runners;

/// <summary>
/// An abstract class to allow creating Specialized Command runners that run Commands through other Commands.
/// </summary>
public abstract class SpecializedCliCommandRunner : ICliCommandRunner
{
    private readonly ICliCommandRunner _commandRunner;
    
    private readonly ICliCommandConfiguration _commandRunnerConfiguration;
    
    /// <summary>
    /// Instantiates the Command Running configuration and the CommandRunner.
    /// </summary>
    /// <param name="commandRunner">The command runner to be used.</param>
    /// <param name="commandRunnerConfiguration">The command running configuration to use for the Command that will run other Commands.</param>
    protected SpecializedCliCommandRunner(ICliCommandRunner commandRunner, ICliCommandConfiguration commandRunnerConfiguration)
    {
        _commandRunner = commandRunner;
        _commandRunnerConfiguration = commandRunnerConfiguration;
    }
    
    /// <summary>
    /// Create the command to be run from the Command runner configuration and an input command.
    /// </summary>
    /// <param name="inputCommand">The command to be run by the Command Runner command.</param>
    /// <returns>The built Command that will run the input command.</returns>
    protected virtual CliCommand CreateRunnerCommand(CliCommand inputCommand)
    {
        ICliCommandBuilder commandBuilder = new CliCommandBuilder(_commandRunnerConfiguration)
            .WithArguments(inputCommand.TargetFilePath + " " + inputCommand.Arguments)
            .WithEnvironmentVariables(inputCommand.EnvironmentVariables)
            .WithProcessResourcePolicy(inputCommand.ResourcePolicy)
            .WithEncoding(inputCommand.StandardInputEncoding,
                inputCommand.StandardOutputEncoding,
                inputCommand.StandardErrorEncoding)
            .WithUserCredential(inputCommand.Credential)
            .WithValidation(inputCommand.ResultValidation)
            .WithAdministratorPrivileges(inputCommand.RequiresAdministrator)
            .WithShellExecution(inputCommand.UseShellExecution)
            .WithWindowCreation(inputCommand.WindowCreation);
        
        return commandBuilder.Build();
    }
    
    /// <summary>
    /// Executes a command asynchronously through a Command Running Command, and returns Command execution information as a CommandResult.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A ProcessResult object containing the execution information of the command.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    public async Task<ProcessResult> ExecuteAsync(CliCommand command, CancellationToken cancellationToken = default)
    {
        CliCommand commandToBeRun = CreateRunnerCommand(command);
        
        return await _commandRunner.ExecuteAsync(commandToBeRun, cancellationToken);
    }

    /// <summary>
    /// Executes a command asynchronously through a CommandRunner Command and returns Command execution information and Command output as a BufferedCommandResult.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A BufferedProcessResult object containing the output of the command that was run.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
    [UnsupportedOSPlatform("ios")]
    [SupportedOSPlatform("android")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("browser")]
#endif
    public async Task<BufferedProcessResult> ExecuteBufferedAsync(CliCommand command, CancellationToken cancellationToken = default)
    {
        CliCommand commandToBeRun = CreateRunnerCommand(command);
        
        return await _commandRunner.ExecuteBufferedAsync(commandToBeRun, cancellationToken);
    }
}