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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using CliRunner.Commands.Abstractions;
using CliRunner.Commands.Buffered;
using CliRunner.Exceptions;

using CliRunner.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif


namespace CliRunner.Commands;

public class CommandRunner : ICommandRunner
{
    private readonly ICommandPipeHandler _commandPipeHandler;

    public CommandRunner(ICommandPipeHandler commandPipeHandler)
    {
        _commandPipeHandler = commandPipeHandler;
    }

    /// <summary>
    /// Creates a process with the specified process start information.
    /// </summary>
    /// <param name="processStartInfo">The process start information to be used to configure the process to be created.</param>
    /// <param name="processorAffinity"></param>
    /// <returns>the newly created Process with the specified start information.</returns>
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
        public Process CreateProcess(ProcessStartInfo processStartInfo, IntPtr processorAffinity = default)
        {
            Process output = new Process
            {
                StartInfo = processStartInfo,
            };
            
            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                output.ProcessorAffinity = processorAffinity;
            }
            
            return output;
        }

        /// <summary>
        /// Creates Process Start Information based on specified parameters and Command object values.
        /// </summary>
        /// <returns>A new ProcessStartInfo object configured with the specified parameters and Command object values. .</returns>
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
        public ProcessStartInfo CreateStartInfo(Command command, bool redirectStandardInput, bool redirectStandardOutput, bool redirectStandardError, bool createNoWindow = false, Encoding encoding = default)
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = command.TargetFilePath,
                WorkingDirectory = command.WorkingDirectoryPath,
                UseShellExecute = command.UseShellExecution,
                CreateNoWindow = createNoWindow,
                RedirectStandardInput = redirectStandardInput,
                RedirectStandardOutput = redirectStandardOutput,
                RedirectStandardError = redirectStandardError,
            };

            if (string.IsNullOrEmpty(command.Arguments) == false)
            {
                output.Arguments = command.Arguments;
            }

            if (command.RequiresAdministrator == true)
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

            if (command.StandardInput != StreamWriter.Null)
            {
                output.RedirectStandardInput = true;
            }
            if (command.StandardOutput != StreamReader.Null)
            {
                output.RedirectStandardOutput = true;
            }
            if (command.StandardError != StreamReader.Null)
            {
                output.RedirectStandardError = true;
            }

            if (command.Credentials != null)
            {
                if (OperatingSystem.IsWindows())
                {
                    if (command.Credentials.Domain != null)
                    {
                        output.Domain = command.Credentials.Domain;
                    }
                    if (command.Credentials.UserName != null)
                    {
                        output.UserName = command.Credentials.UserName;
                    }
                    if (command.Credentials.Password != null)
                    {
                        output.Password = command.Credentials.Password;
                    }
                    
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    if (command.Credentials.LoadUserProfile != null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                    {
#if NETSTANDARD2_0
                    output.LoadUserProfile = command.Credentials.LoadUserProfile;
#else
                        output.LoadUserProfile = (bool)command.Credentials.LoadUserProfile;
#endif
                    }
                }
            }

            if (command.EnvironmentVariables != null)
            {
                foreach (KeyValuePair<string, string> variable in command.EnvironmentVariables)
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
        
        private void TargetExecutablePathCheck(ProcessStartInfo processStartInfo)
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
        private async Task DoPipingInputWorkIfNeeded(Command command, Process process)
        {
            if (process.StartInfo.RedirectStandardInput == true)
            {
                await _commandPipeHandler.PipeStandardInputAsync(process, command);
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
        private async Task DoPipingOutputWorkIfNeeded(Command command, Process process)
        {
            if (process.StartInfo.RedirectStandardOutput == true)
            {
                await _commandPipeHandler.PipeStandardOutputAsync(process, command);
            }
            if (process.StartInfo.RedirectStandardError == true)
            {
                await _commandPipeHandler.PipeStandardErrorAsync(process, command);
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
        private void CheckIfUnsuccessfulExecutionRequiresException(Command command, Process process)
        {
            if (process.ExitCode != 0 && command.ResultValidation == CommandResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccessfulException(process.ExitCode, command);
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
        private async Task DoCommonCommandExecutionWork(Command command, Process process, CancellationToken cancellationToken)
        {
            TargetExecutablePathCheck(process.StartInfo);
            await DoPipingInputWorkIfNeeded(command, process);
            
            process.Start();
            
            // Wait for process to exit before redirecting Standard Output and Standard Error.
            await process.WaitForExitAsync(cancellationToken);

            CheckIfUnsuccessfulExecutionRequiresException(command, process);
            
            await DoPipingOutputWorkIfNeeded(command, process);
        }

        /// <summary>
        /// Executes a command asynchronously and returns Command execution information as a CommandResult.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>A CommandResult object containing the execution information of the command.</returns>
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
        public async Task<CommandResult> ExecuteAsync(Command command, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(command, Encoding.Default, cancellationToken);
        }

        /// <summary>
        /// Executes a command asynchronously and returns Command execution information as a CommandResult.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="encoding">The encoding to use for the command input (if applicable) and output.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>A CommandResult object containing the execution information of the command.</returns>
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
        public async Task<CommandResult> ExecuteAsync(Command command, Encoding encoding, CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(CreateStartInfo(command, command.StandardInput != StreamWriter.Null,
                command.StandardOutput != StreamReader.Null,
                command.StandardError != StreamReader.Null, command.WindowCreation));
            
            await DoCommonCommandExecutionWork(command, process, cancellationToken);
            
            return new CommandResult(process.ExitCode, process.StartTime, process.ExitTime);
        }

        /// <summary>
        /// Executes a command asynchronously and returns Command execution information and Command output as a BufferedCommandResult.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>A BufferedCommandResult object containing the output of the command.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the executable's specified file path is not found.</exception>
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
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(Command command, CancellationToken cancellationToken = default)
        {
            return await ExecuteBufferedAsync(command, Encoding.Default, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Executes a command asynchronously and returns Command execution information and Command output as a BufferedCommandResult.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <param name="encoding">The encoding to use for the command input (if applicable) and output.</param>
        /// <returns>A BufferedCommandResult object containing the output of the command.</returns>
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
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(Command command, Encoding encoding, CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(CreateStartInfo(command,
                command.StandardInput != null,
                true, true,
                command.WindowCreation, encoding));

            await DoCommonCommandExecutionWork(command, process, cancellationToken);
            
#if NET6_0_OR_GREATER
                return new BufferedCommandResult(process.ExitCode,
 await process.StandardOutput.ReadToEndAsync(cancellationToken),
                    await process.StandardError.ReadToEndAsync(cancellationToken),
                    process.StartTime, process.ExitTime);
#else
                return new BufferedCommandResult(process.ExitCode, 
                await process.StandardOutput.ReadToEndAsync(),
                await process.StandardError.ReadToEndAsync(), 
                process.StartTime, process.ExitTime);
#endif
        }
}