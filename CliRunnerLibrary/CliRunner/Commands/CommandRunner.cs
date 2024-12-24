/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Command.Execution.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Command.Execution.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using AlastairLundy.Extensions.Processes;
#endif

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;

using System;
#endif

using CliRunner.Commands.Abstractions;
using CliRunner.Commands.Buffered;

using CliRunner.Exceptions;

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
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public Process CreateProcess(ProcessStartInfo processStartInfo)
        {
            Process output = new Process
            {
                StartInfo = processStartInfo,
            };

            if (OperatingSystem.IsWindows() | OperatingSystem.IsLinux())
            {
                output.ProcessorAffinity = ProcessorAffinity;
            }
            
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public ProcessStartInfo CreateStartInfo(bool redirectStandardInput, bool redirectStandardOutput, bool redirectStandardError, Encoding encoding = default)
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = TargetFilePath,
                WorkingDirectory = WorkingDirectoryPath,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardInput = redirectStandardInput,
                RedirectStandardOutput = redirectStandardOutput,
                RedirectStandardError = redirectStandardError,
            };

            if (string.IsNullOrEmpty(Arguments) == false)
            {
                output.Arguments = Arguments;
            }

            if (RequiresAdministrator == true)
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

            if (StandardInput != StreamWriter.Null)
            {
                output.RedirectStandardInput = true;
            }
            if (StandardOutput != StreamReader.Null)
            {
                output.RedirectStandardOutput = true;
            }
            if (StandardError != StreamReader.Null)
            {
                output.RedirectStandardError = true;
            }

            if (Credentials != null)
            {
                if (Credentials.Domain != null && OperatingSystem.IsWindows())
                {
                    output.Domain = Credentials.Domain;
                }
                if (Credentials.UserName != null)
                {
                    output.UserName = Credentials.UserName;
                }
                if (Credentials.Password != null && OperatingSystem.IsWindows())
                {
                    output.Password = Credentials.Password;
                }
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (Credentials.LoadUserProfile != null && OperatingSystem.IsWindows())
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
#if NETSTANDARD2_0
                    output.LoadUserProfile = Credentials.LoadUserProfile;
#else
                   output.LoadUserProfile = (bool)Credentials.LoadUserProfile;
#endif
                }
            }

            if (EnvironmentVariables != null)
            {
                foreach (var variable in EnvironmentVariables)
                {
                    if (variable.Value != null)
                    {
                        output.Environment[variable.Key] = variable.Value;
                    }
                }
            }
            
            output.UseShellExecute = UseShellExecution;

            if (redirectStandardInput)
            {
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
                output.StandardInputEncoding = Encoding.Default;
#endif
            }
            
            output.StandardOutputEncoding = encoding ?? Encoding.Default;
            output.StandardErrorEncoding = encoding ?? Encoding.Default;
            
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(
                CreateStartInfo(false, false, false));
            
            if (process.StartInfo.RedirectStandardInput == true)
            {
                await PipeStandardInputAsync(process);
            }
            
            process.Start();
            
            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0 && this.ResultValidation == CommandResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccesfulException(process.ExitCode, this);
            }
            
            if (process.StartInfo.RedirectStandardOutput == true)
            {
                await PipeStandardOutputAsync(process);
            }
            if (process.StartInfo.RedirectStandardError == true)
            {
                await PipeStandardErrorAsync(process);
            }
            
            return new CommandResult(process.ExitCode, process.StartTime, process.ExitTime);
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
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(CancellationToken cancellationToken = default)
        {
            return await ExecuteBufferedAsync(Encoding.Default, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(Encoding encoding, CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(
                CreateStartInfo(StandardInput != null, true, true, encoding));
            
            if (process.StartInfo.RedirectStandardInput == true)
            {
                await PipeStandardInputAsync(process);
            }
            
            process.Start();
            
            await process.WaitForExitAsync(cancellationToken);
            
            if (process.ExitCode != 0 && ResultValidation == CommandResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccesfulException(process.ExitCode, this);
            }
            

            if (process.StartInfo.RedirectStandardOutput == true)
            {
                await PipeStandardOutputAsync(process);
            }
            if (process.StartInfo.RedirectStandardError == true)
            {
                await PipeStandardErrorAsync(process);
            }
            
#if NET6_0_OR_GREATER
            return new BufferedCommandResult(process.ExitCode, await process.StandardOutput.ReadToEndAsync(cancellationToken),
                await process.StandardError.ReadToEndAsync(cancellationToken),
                process.StartTime, process.ExitTime);
#else
            return new BufferedCommandResult(process.ExitCode, 
                await process.StandardOutput.ReadToEndAsync(), await process.StandardError.ReadToEndAsync(), 
            process.StartTime, process.ExitTime);
#endif
        }
    }
}