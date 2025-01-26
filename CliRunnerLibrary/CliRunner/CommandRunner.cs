/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Command.Execution.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Command.Execution.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

// ReSharper disable RedundantBoolCompare
// ReSharper disable ConvertToPrimaryConstructor
// ReSharper disable SuggestVarOrType_SimpleTypes

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Exceptions;
using CliRunner.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif


namespace CliRunner;

/// <summary>
/// 
/// </summary>
public class CommandRunner : ICommandRunner
{
    private readonly ICommandPipeHandler _commandPipeHandler;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandPipeHandler"></param>
    public CommandRunner(ICommandPipeHandler commandPipeHandler)
    {
        _commandPipeHandler = commandPipeHandler;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandPipeHandler"></param>
    /// <returns></returns>
    public static CommandRunner CreateInstance(ICommandPipeHandler commandPipeHandler)
    {
        return new CommandRunner(commandPipeHandler);
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
    /// Creates Process Start Information based on specified Command object values.
    /// </summary>
    /// <param name="command">The command object to specify Process info.</param>
    /// <returns>A new ProcessStartInfo object configured with the specified Command object values. .</returns>
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
    public ProcessStartInfo CreateStartInfo(Command command)
    {
        return CreateStartInfo(command, command.StandardOutput != null, command.StandardError != null);
    }

    /// <summary>
    /// Creates Process Start Information based on specified parameters and Command object values.
    /// </summary>
    /// <param name="command">The command object to specify Process info.</param>
    /// <param name="redirectStandardOutput">Whether to redirect the Standard Output.</param>
    /// <param name="redirectStandardError">Whether to redirect the Standard Error.</param>
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
        public ProcessStartInfo CreateStartInfo(Command command, bool redirectStandardOutput,
            bool redirectStandardError)
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = command.TargetFilePath,
                WorkingDirectory = command.WorkingDirectoryPath,
                UseShellExecute = command.UseShellExecution,
                CreateNoWindow = command.WindowCreation,
                RedirectStandardInput = command.StandardInput != null,
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
                        output.LoadUserProfile = (bool)command.Credentials.LoadUserProfile;
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
            
            if (output.RedirectStandardInput == true)
            {
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
                output.StandardInputEncoding = command.StandardInputEncoding ?? Encoding.Default;
#endif
            }
            
            output.StandardOutputEncoding = command.StandardOutputEncoding ?? Encoding.Default;
            output.StandardErrorEncoding = command.StandardErrorEncoding ?? Encoding.Default;
            
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
        private async Task DoCommonCommandExecutionWork(Command command, Process process, CancellationToken cancellationToken)
        {
            if (File.Exists(process.StartInfo.FileName) == false)
            {
                throw new FileNotFoundException(Resources.Exceptions_FileNotFound.Replace("{file}", process.StartInfo.FileName));
            }
            
            // Handle input piping if needed.
            if (process.StartInfo.RedirectStandardInput == true)
            {
                await _commandPipeHandler.PipeStandardInputAsync(command, process);
            }
            
            process.Start();
            
            // Wait for process to exit before redirecting Standard Output and Standard Error.
            await process.WaitForExitAsync(cancellationToken);

            // Throw a CommandNotSuccessful exception if required.
            if (process.ExitCode != 0 && command.ResultValidation == CommandResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccessfulException(process.ExitCode, command);
            }
            
            // Handle output piping if needed.
            if (process.StartInfo.RedirectStandardOutput == true)
            {
                await _commandPipeHandler.PipeStandardOutputAsync(process, command);
            }
            if (process.StartInfo.RedirectStandardError == true)
            {
                await _commandPipeHandler.PipeStandardErrorAsync(process, command);
            }
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
            Process process = CreateProcess(CreateStartInfo(command));
            
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
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("android")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("browser")]
#endif
        public async Task<BufferedCommandResult> ExecuteBufferedAsync(Command command,
            CancellationToken cancellationToken = default)
        {
            Process process = CreateProcess(CreateStartInfo(command,
                true, true));

            await DoCommonCommandExecutionWork(command, process, cancellationToken);
            
            return new BufferedCommandResult(process.ExitCode,
 await process.StandardOutput.ReadToEndAsync(cancellationToken),
                    await process.StandardError.ReadToEndAsync(cancellationToken),
                    process.StartTime, process.ExitTime);
        }
}