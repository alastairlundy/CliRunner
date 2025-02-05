/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CliRunner.Abstractions;

/// <summary>
/// An interface to specify required Command Running functionality.
/// </summary>
public interface ICommandRunner
{
    /// <summary>
    /// Creates a process with the specified process start information.
    /// </summary>
    /// <param name="processStartInfo">The process start information to be used to configure the process to be created.</param>
    /// <param name="processorAffinity"></param>
    /// <returns>The newly created Process with the specified start information.</returns>
    Process CreateProcess(ProcessStartInfo processStartInfo, IntPtr processorAffinity = default);

    /// <summary>
    /// Creates Process Start Information based on specified Command object values.
    /// </summary>
    /// <param name="command">The command object to specify Process info.</param>
    /// <returns>A new ProcessStartInfo object configured with the specified Command object values. .</returns>
    ProcessStartInfo CreateStartInfo(Command command);

    /// <summary>
    /// Creates Process Start Information based on specified parameters and Command object values.
    /// </summary>
    /// <param name="command">The command object to specify Process info.</param>
    /// <param name="redirectStandardOutput">Whether to redirect the Standard Output.</param>
    /// <param name="redirectStandardError">Whether to redirect the Standard Error.</param>
    /// <returns>A new ProcessStartInfo object configured with the specified parameters and Command object values.</returns>
    ProcessStartInfo CreateStartInfo(Command command, bool redirectStandardOutput, bool redirectStandardError);
    
    /// <summary>
    /// Executes a command asynchronously and returns Command execution information as a CommandResult.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A CommandResult object containing the execution information of the command.</returns>
    Task<CommandResult> ExecuteAsync(Command command, CancellationToken cancellationToken = default);
        
    /// <summary>
    ///Executes a command asynchronously and returns Command execution information and Command output as a BufferedCommandResult.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A BufferedCommandResult object containing the output of the command.</returns>
    Task<BufferedCommandResult> ExecuteBufferedAsync(Command command, CancellationToken cancellationToken = default);
}