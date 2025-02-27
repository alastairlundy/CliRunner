﻿/*
    CliInvoke 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Threading;
using System.Threading.Tasks;

using AlastairLundy.Extensions.Processes;

// ReSharper disable once CheckNamespace
namespace AlastairLundy.CliInvoke.Abstractions;

/// <summary>
/// An interface to specify required Command Running functionality.
/// </summary>
public interface ICliCommandInvoker
{
    /// <summary>
    /// Executes a command asynchronously and returns Command execution information as a CommandResult.
    /// </summary>
    /// <param name="commandConfiguration">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A CommandResult object containing the execution information of the command.</returns>
    Task<ProcessResult> ExecuteAsync(CliCommandConfiguration commandConfiguration, CancellationToken cancellationToken = default);
        
    /// <summary>
    ///Executes a command asynchronously and returns Command execution information and Command output as a BufferedCommandResult.
    /// </summary>
    /// <param name="commandConfiguration">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A BufferedCommandResult object containing the output of the command.</returns>
    Task<BufferedProcessResult> ExecuteBufferedAsync(CliCommandConfiguration commandConfiguration, CancellationToken cancellationToken = default);
}