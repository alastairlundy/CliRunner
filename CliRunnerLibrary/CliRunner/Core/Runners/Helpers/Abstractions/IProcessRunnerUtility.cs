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

namespace CliRunner.Runners.Helpers.Abstractions;

/// <summary>
/// A Process Running Utility interface to easily create different Process Runners.
/// </summary>
/// <remarks>This interface is primarily intended for internal use OR use when creating a Process Runner or Command Runner implementation.</remarks>
public interface IProcessRunnerUtility
{
    /// <summary>
    /// Starts a Process and asynchronously waits for it to exit before returning.
    /// </summary>
    /// <param name="process">The process to be executed.</param>
    /// <param name="cancellationToken">The cancellation token to use to cancel the waiting for process exit if required.</param>
    /// <returns>The awaited task.</returns>
    public Task ExecuteAsync(Process process, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disposes of the specified process.
    /// </summary>
    /// <param name="process">The process to be disposed of.</param>
    public void DisposeOfProcess(Process process);
    
    /// <summary>
    /// Gets the results from an exited Process.
    /// </summary>
    /// <param name="process">The process to retrieve results from.</param>
    /// <param name="disposeOfProcess">Whether to dispose of the Process before returning.</param>
    /// <returns>The results from an exited process.</returns>
    public ProcessResult GetResult(Process process, bool disposeOfProcess); 
    
    /// <summary>
    /// Gets the BufferedProcessResults results from an exited Process.
    /// </summary>
    /// <param name="process">The process to retrieve results from.</param>
    /// <param name="disposeOfProcess">Whether to dispose of the Process before returning.</param>
    /// <returns>The results from an exited process.</returns>
    public BufferedProcessResult GetBufferedResult(Process process, bool disposeOfProcess);
    
    /// <summary>
    /// Asynchronously gets the ProcessResult results from an exited Process.
    /// </summary>
    /// <param name="process">The process to retrieve results from.</param>
    /// <param name="disposeOfProcess">Whether to dispose of the Process before returning.</param>
    /// <returns>The results from an exited process.</returns>
    public Task<ProcessResult> GetResultAsync(Process process, bool disposeOfProcess);
    
    /// <summary>
    /// Asynchronously gets the BufferedProcessResult results from an exited Process.
    /// </summary>
    /// <param name="process">The process to retrieve results from.</param>
    /// <param name="disposeOfProcess">Whether to dispose of the Process before returning.</param>
    /// <returns>The results from an exited process.</returns>
    public Task<BufferedProcessResult> GetBufferedResultAsync(Process process, bool disposeOfProcess);
    
}