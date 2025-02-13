/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CliRunner.Runners.Abstractions;

/// <summary>
/// An interface to specify safe Process Running functionality.
/// </summary>
public interface IProcessRunner
{
    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <remarks>Use the Async version of this method where possible to avoid UI freezes and other potential issues.</remarks>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <returns>The Process Results from the running the process.</returns>
    public ProcessResult ExecuteProcess(Process process, ProcessResultValidation processResultValidation);

    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <remarks>Use the Async version of this method where possible to avoid UI freezes and other potential issues.</remarks>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
    public BufferedProcessResult ExecuteBufferedProcess(Process process, ProcessResultValidation processResultValidation);

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Process Results from the running the process.</returns>
    public Task<ProcessResult> ExecuteProcessAsync(Process process, ProcessResultValidation processResultValidation,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
    public Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process,
        ProcessResultValidation processResultValidation, CancellationToken cancellationToken = default);
}