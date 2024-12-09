/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;

using System.Threading;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using CliRunner.Commands.Abstractions;
using CliRunner.Commands.Extensions;
using CliRunner.Piping;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Commands
{
    public partial class Command : ICommandRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
#endif
        public Process CreateProcess()
        {
            ProcessStartInfo startInfo = CreateStartInfo();

            Process output = new Process
            {
                StartInfo = startInfo,
                
            };
            
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public ProcessStartInfo CreateStartInfo()
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = TargetFilePath,
                WorkingDirectory = WorkingDirectoryPath,
                UseShellExecute = false,
                CreateNoWindow = false,
            };

            if (string.IsNullOrEmpty(Arguments) == false)
            {
                output.Arguments = Arguments;
            }

            if (RunAsAdministrator == true)
            {
                if (OperatingSystem.IsWindows())
                {
                    output.Verb = "runas";
                }
                else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS() || OperatingSystem.IsFreeBSD())
                {
                    output.Verb = "sudo";
                }
            }

            if (StandardInputPipe != PipeSource.Null)
            {
                output.RedirectStandardInput = true;
            }
            if (StandardOutputPipe != PipeTarget.Null)
            {
                output.RedirectStandardOutput = true;
            }
            if (StandardErrorPipe != PipeTarget.Null)
            {
                output.RedirectStandardError = true;
            }

            return output;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public CommandResult Execute()
        {
            return ExecuteBuffered().ToCommandResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public BufferedCommandResult ExecuteBuffered()
        {
            Process process = CreateProcess();
            process.StartInfo = CreateStartInfo();

            process.Start();
            
            process.WaitForExit();
           
#if NET6_0_OR_GREATER
            return new BufferedCommandResult(process.ExitCode, process.StandardInput.ToString()!,
                process.StandardOutput.ReadToEnd(),
                process.StartTime, process.ExitTime);
#else
            return new BufferedCommandResult(process.ExitCode, process.StandardInput.ToString(),
                process.StandardOutput.ReadToEnd(),
                process.StartTime, process.ExitTime);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
           var result = await ExecuteBufferedAsync(cancellationToken);
           return result.ToCommandResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess();
            process.StartInfo = CreateStartInfo();
            process.Start();
            
#if NET6_0_OR_GREATER
            await process.WaitForExitAsync(cancellationToken);
#else
            process.WaitForExit();
#endif
            
#if NET6_0_OR_GREATER
            return new BufferedCommandResult(process.ExitCode, 
                process.StandardInput.ToString()!, await process.StandardOutput.ReadToEndAsync(cancellationToken), process.StartTime, process.ExitTime);
#else
            return new BufferedCommandResult(process.ExitCode, 
                process.StandardInput.ToString(), await process.StandardOutput.ReadToEndAsync(), process.StartTime, process.ExitTime);
#endif
}
    }
}