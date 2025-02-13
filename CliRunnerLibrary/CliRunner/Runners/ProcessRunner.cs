/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Exceptions;
using CliRunner.Internal.Localizations;

using CliRunner.Runners.Abstractions;

namespace CliRunner.Runners;

/// <summary>
/// The default implementation of IProcessRunner, a safer way to execute processes.
/// </summary>
public class ProcessRunner : IProcessRunner
{
    public ProcessRunner()
    {
        
    }

    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <remarks>Use the Async version of this method where possible to avoid UI freezes and other potential issues.</remarks>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <returns>The Process Results from the running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
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
    public ProcessResult ExecuteProcess(Process process, ProcessResultValidation processResultValidation)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }
        
        process.Start();

        process.WaitForExit();

        if (processResultValidation == ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }
        
        ProcessResult processResult = new ProcessResult(process.ExitCode, process.StartTime, process.ExitTime);
       
        // Properly dispose of the Process once the results are gotten.
        process.Close();
        process.Dispose();
       
        // Return after process disposal.
        return processResult;
    }

    /// <summary>
    /// Runs the process synchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <remarks>Use the Async version of this method where possible to avoid UI freezes and other potential issues.</remarks>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
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
    public BufferedProcessResult ExecuteBufferedProcess(Process process,
        ProcessResultValidation processResultValidation)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }
        
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        process.Start();

        process.WaitForExit();
       
        if (processResultValidation == ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }
        
        BufferedProcessResult processResult = new BufferedProcessResult(process.ExitCode,
            process.StandardOutput.ReadToEnd(),
            process.StandardError.ReadToEnd(),
            process.StartTime,
            process.ExitTime);
       
        process.Close();
        process.Dispose();
       
        return processResult;
    }

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Process Results from the running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
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
    public async Task<ProcessResult> ExecuteProcessAsync(Process process,
        ProcessResultValidation processResultValidation,
        CancellationToken cancellationToken = default)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }
        
        process.Start();
       
       await process.WaitForExitAsync(cancellationToken);
       
       if (processResultValidation == ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
       {
           throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
       }
       
       ProcessResult processResult = new ProcessResult(process.ExitCode, process.StartTime, process.ExitTime);
       
       process.Close();
       process.Dispose();
       
       return processResult;
    }

    

    /// <summary>
    /// Runs the process asynchronously, waits for exit, and safely disposes of the Process before returning.
    /// </summary>
    /// <param name="process">The process to be run.</param>
    /// <param name="processResultValidation">The process result validation to be used.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>The Buffered Process Results from running the process.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file, with the file name of the process to be executed, is not found.</exception>
    /// <exception cref="ProcessNotSuccessfulException">Thrown if the result validation requires the process to exit with exit code zero and the process exits with a different exit code.</exception>
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
    public async Task<BufferedProcessResult> ExecuteBufferedProcessAsync(Process process, 
        ProcessResultValidation processResultValidation,
        CancellationToken cancellationToken = default)
    {
        if (File.Exists(process.StartInfo.FileName) == false)
        {
            throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", process.StartInfo.FileName));
        }

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        
        process.Start();
       
        await process.WaitForExitAsync(cancellationToken);

        if (processResultValidation == ProcessResultValidation.ExitCodeZero && process.ExitCode != 0)
        {
            throw new ProcessNotSuccessfulException(process: process, exitCode: process.ExitCode);
        }
        
        BufferedProcessResult output = new BufferedProcessResult(process.ExitCode,
           await process.StandardOutput.ReadToEndAsync(cancellationToken),
            await process.StandardError.ReadToEndAsync(cancellationToken), process.StartTime, process.ExitTime);
        
        process.Close();
        process.Dispose();

        return output;
    }
}