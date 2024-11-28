/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Commands.Abstractions;
using CliRunner.Piping.Abstractions;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Commands
{
    public class CommandRunner : ICommandRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
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
        public Process CreateProcess(Command command)
        {
            ProcessStartInfo startInfo = GetStartInfo(command);

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
        public ProcessStartInfo GetStartInfo(Command command)
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = command.TargetFilePath,
                WorkingDirectory = command.WorkingDirectoryPath,
                UseShellExecute = false,
                CreateNoWindow = false,
            };

            if (string.IsNullOrEmpty(command.Arguments) == false)
            {
                output.Arguments = command.Arguments;
            }

            if (command.RunAsAdministrator == true)
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

            if (command.StandardInputPipe != command.StandardInputPipe.Null)
            {
                output.RedirectStandardInput = true;
            }
            if (command.StandardOutputPipe != command.StandardOutputPipe.Null)
            {
                output.RedirectStandardOutput = true;
            }
            if (command.StandardErrorPipe != command.StandardErrorPipe.Null)
            {
                output.RedirectStandardError = true;
            }

            return output;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
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
        public CommandResult Execute(Command command)
        {
            Process process = CreateProcess(command);

            process.Start();
            
            process.WaitForExit();
            
            return new CommandResult(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StartTime, process.ExitTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
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
        public async Task<CommandResult> ExecuteAsync(Command command, CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(command);

            process.Start();
            
#if NET6_0_OR_GREATER
            await process.WaitForExitAsync(cancellationToken);
#else
            process.WaitForExit();
#endif

            return new CommandResult(process.ExitCode, 
#if NET6_0_OR_GREATER
                await process.StandardOutput.ReadToEndAsync(cancellationToken)
#else
                await process.StandardOutput.ReadToEndAsync()
#endif
, 
                process.StartTime, process.ExitTime);
        }
    }
}