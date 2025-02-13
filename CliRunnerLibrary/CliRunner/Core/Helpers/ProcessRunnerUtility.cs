/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CliRunner.Internal.Localizations;
using CliRunner.Runners.Helpers.Abstractions;

namespace CliRunner.Runners.Helpers;

/// <summary>
/// A Process Running Utility to easily create different Process Runners.
/// </summary>
/// <remarks>This class is primarily intended for internal use OR use when creating a Process Runner or Command Runner implementation.</remarks>
public class ProcessRunnerUtility : IProcessRunnerUtility
{
    
    public async Task ExecuteAsync(Process process, CancellationToken cancellationToken = default)
    {
        if (process.HasExited == false)
        {
            process.Start();
            
            await process.WaitForExitAsync(cancellationToken);
        }
        else
        {
            throw new InvalidOperationException(Resources.Exceptions_Processes_CannotStartExitedProcess);
        }
    }
    
    /// <summary>
    /// Disposes of the Process if it is running 
    /// </summary>
    /// <param name="process"></param>
    public void DisposeOfProcess(Process process)
    {
        if (process.HasExited == false)
        {
            process.Kill();
        }
        
        process.Close();
        process.Dispose();
    }

    
    public ProcessResult GetResult(Process process, bool disposeOfProcess)
    {
        if (process.HasExited == false)
        {
            if (Process.GetProcesses().Any(x => x.Equals(process)) == false)
            {
                 process.Start();
            }
            
            process.WaitForExit();
        }
        
        ProcessResult processResult = new ProcessResult(process.ExitCode, process.StartTime, process.ExitTime);

        if (disposeOfProcess)
        {
            DisposeOfProcess(process);
        }
        
        return processResult;
    }

    public BufferedProcessResult GetBufferedResult(Process process, bool disposeOfProcess)
    {
        if (process.HasExited == false)
        {
            if (Process.GetProcesses().Any(x => x.Equals(process)) == false)
            {
                process.Start();
            }
            
            process.WaitForExit();
        }
        
        BufferedProcessResult processResult = new BufferedProcessResult(process.ExitCode,
             process.StandardOutput.ReadToEnd(),  process.StandardError.ReadToEnd(),
            process.StartTime, process.ExitTime);

        if (disposeOfProcess)
        {
            DisposeOfProcess(process);
        }
        
        return processResult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="disposeOfProcess"></param>
    /// <returns></returns>
    public async Task<ProcessResult> GetResultAsync(Process process, bool disposeOfProcess)
    {
        if (process.HasExited == false)
        {
            if (Process.GetProcesses().Any(x => x.Equals(process)) == false)
            {
               await ExecuteAsync(process);
            }
            else
            {
                await process.WaitForExitAsync();
            }
        }
        
        ProcessResult processResult = new ProcessResult(process.ExitCode, process.StartTime, process.ExitTime);

        if (disposeOfProcess)
        {
            DisposeOfProcess(process);
        }
        
        return processResult;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="process"></param>
    /// <param name="disposeOfProcess">Whether to dispose of the process before returning.</param>
    /// <returns>The BufferedProcessResult </returns>
    public async Task<BufferedProcessResult> GetBufferedResultAsync(Process process, bool disposeOfProcess)
    {
        if (process.HasExited == false)
        {
            if (Process.GetProcesses().Contains(process) == false)
            {
                process.Start();
            }
            
            await process.WaitForExitAsync();
        }
        
        BufferedProcessResult processResult = new BufferedProcessResult(process.ExitCode,
            await process.StandardOutput.ReadToEndAsync(), await process.StandardError.ReadToEndAsync(),
            process.StartTime, process.ExitTime);

        if (disposeOfProcess)
        {
            DisposeOfProcess(process);
        }
        
        return processResult;
    }
}