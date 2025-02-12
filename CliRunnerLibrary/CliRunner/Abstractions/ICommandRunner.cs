/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Threading;
using System.Threading.Tasks;

namespace CliRunner.Abstractions;

/// <summary>
/// An interface to specify required Command Running functionality.
/// </summary>
public interface ICommandRunner
{
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