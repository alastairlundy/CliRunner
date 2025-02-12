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
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Abstractions;
using CliRunner.Exceptions;

using CliRunner.Internal.Localizations;
using CliRunner.Piping.Abstractions;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace CliRunner;

/// <summary>
/// The default implementation of the CliRunner command running mechanism, ICommandRunner.
/// </summary>
public class CommandRunner : ICommandRunner
{
        private readonly ICommandPipeHandler _commandPipeHandler;
        private readonly IProcessCreator _processCreator;

        /// <summary>
        /// Initialises the CommandRunner with the ICommandPipeHandler to be used.
        /// </summary>
        /// <param name="commandPipeHandler">The ICommandPipeHandler to be used.</param>
        /// <param name="processCreator"></param>
        public CommandRunner(ICommandPipeHandler commandPipeHandler, IProcessCreator processCreator)
        {
            _commandPipeHandler = commandPipeHandler;
            _processCreator = processCreator;
        }
    

        /// <summary>
        /// Performs common Command Execution tasks shared by the ExecuteAsync and ExecuteBufferedAsync methods.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="process">The process to be piped into and out of the Command.</param>
        /// <param name="cancellationToken">The cancellation token to use in case of Cancellation.</param>
        /// <exception cref="FileNotFoundException">Thrown if the Target File Path is not found.</exception>
        /// <exception cref="CommandNotSuccessfulException">Thrown if the Command execution fails and if the Command requires an exception to be thrown if Command Execution fails.</exception>
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
            if (process.ExitCode != 0 && command.ResultValidation == ProcessResultValidation.ExitCodeZero)
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

        private void DisposeOfProcess(Process process)
        {
            process.Close();
            process.Dispose();
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
        public async Task<ProcessResult> ExecuteAsync(Command command, CancellationToken cancellationToken = default)
        {
            Process process = _processCreator.CreateProcess(_processCreator.CreateStartInfo(command), command.ResourcePolicy);
            
            await DoCommonCommandExecutionWork(command, process, cancellationToken);
            
            ProcessResult commandResult = new ProcessResult(process.ExitCode, process.StartTime, process.ExitTime);
            
            DisposeOfProcess(process);
            
            return commandResult;
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
            Process process = _processCreator.CreateProcess(_processCreator.CreateStartInfo(command,
                true, true), command.ResourcePolicy);

            await DoCommonCommandExecutionWork(command, process, cancellationToken);
            
            BufferedCommandResult commandResult = new BufferedCommandResult(process.ExitCode,
 await process.StandardOutput.ReadToEndAsync(cancellationToken),
                    await process.StandardError.ReadToEndAsync(cancellationToken),
                    process.StartTime, process.ExitTime);
            
            DisposeOfProcess(process);
            
            return commandResult;
        }
}