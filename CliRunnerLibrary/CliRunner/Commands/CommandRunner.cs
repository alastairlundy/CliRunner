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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;

using System;
#endif

using CliRunner.Commands.Abstractions;
using CliRunner.Commands.Buffered;

using CliRunner.Exceptions;

using CliRunner.Internal.Localizations;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Commands
{
    public partial class Command : ICommandRunner
    {
        /// <summary>
        /// Creates a process with the specified process start information.
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
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
        [SupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("ios")]
        [SupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public ProcessStartInfo CreateStartInfo(bool redirectStandardInput, bool redirectStandardOutput, bool redirectStandardError, bool createNoWindow = false, Encoding encoding = default)
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = TargetFilePath,
                WorkingDirectory = WorkingDirectoryPath,
                UseShellExecute = UseShellExecution,
                CreateNoWindow = createNoWindow,
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
                if (OperatingSystem.IsWindows())
                {
                    if (Credentials.Domain != null)
                    {
                        output.Domain = Credentials.Domain;
                    }
                    if (Credentials.UserName != null)
                    {
                        output.UserName = Credentials.UserName;
                    }
                    if (Credentials.Password != null)
                    {
                        output.Password = Credentials.Password;
                    }
                    
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    if (Credentials.LoadUserProfile != null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    {
#if NETSTANDARD2_0
                    output.LoadUserProfile = Credentials.LoadUserProfile;
#else
                        output.LoadUserProfile = (bool)Credentials.LoadUserProfile;
#endif
                    }
                }
            }

            if (EnvironmentVariables != null)
            {
                foreach (KeyValuePair<string, string> variable in EnvironmentVariables)
                {
                    if (variable.Value != null)
                    {
                        output.Environment[variable.Key] = variable.Value;
                    }
                }
            }
            
            if (redirectStandardInput == true)
            {
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
                output.StandardInputEncoding = Encoding.Default;
#endif
            }
            
            output.StandardOutputEncoding = encoding ?? Encoding.Default;
            output.StandardErrorEncoding = encoding ?? Encoding.Default;
            
            return output;
        }

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
        private void CheckTargetExecutableExists(ProcessStartInfo processStartInfo)
        {
            if (File.Exists(processStartInfo.FileName) == false)
            {
                throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", processStartInfo.FileName));
            }
        }

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
        private async Task DoPipingInputWorkIfNeeded(Process process)
        {
            if (process.StartInfo.RedirectStandardInput == true)
            {
                await PipeStandardInputAsync(process);
            }
        }
        
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
        private async Task DoPipingOutputWorkIfNeeded(Process process)
        {
            if (process.StartInfo.RedirectStandardOutput == true)
            {
                await PipeStandardOutputAsync(process);
            }
            if (process.StartInfo.RedirectStandardError == true)
            {
                await PipeStandardErrorAsync(process);
            }
        }

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
        private void CheckIfUnsuccessfulExecutionRequiresException(Process process)
        {
            if (process.ExitCode != 0 && ResultValidation == CommandResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccessfulException(process.ExitCode, this);
            }
        }

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
        private async Task DoCommonCommandExecutionWork(Process process, CancellationToken cancellationToken)
        {
            CheckTargetExecutableExists(process.StartInfo);
            await DoPipingInputWorkIfNeeded(process);
            
            process.Start();
            
            // Wait for process to exit before redirecting Standard Output and Standard Error.
            await process.WaitForExitAsync(cancellationToken);

            CheckIfUnsuccessfulExecutionRequiresException(process);
            
            await DoPipingOutputWorkIfNeeded(process);
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
        [SupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("ios")]
        [SupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(Encoding.Default, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Thrown if the executable's specified file path is not found.</exception>
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
        public async Task<CommandResult> ExecuteAsync(Encoding encoding, CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(
                CreateStartInfo(false, false, false, WindowCreation));
            
            await DoCommonCommandExecutionWork(process, cancellationToken);
            
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
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("android")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("tvos")]
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
        /// <exception cref="FileNotFoundException">Thrown if the executable's specified file path is not found.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("android")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(Encoding encoding, CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(
                CreateStartInfo(StandardInput != null, true, true, WindowCreation, encoding));

            await DoCommonCommandExecutionWork(process, cancellationToken);
            
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