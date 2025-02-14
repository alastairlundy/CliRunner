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
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Abstractions;
using CliRunner.Exceptions;
using CliRunner.Piping.Abstractions;
using CliRunner.Runners.Abstractions;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

// ReSharper disable CheckNamespace
namespace CliRunner;

/// <summary>
/// The default implementation of the CliRunner command running mechanism, ICommandRunner.
/// </summary>
public class CommandRunner : ICommandRunner
{
        private readonly IPipedProcessRunner _pipedProcessRunner;
        
        private readonly IProcessPipeHandler _processPipeHandler;
        
        private readonly IProcessCreator _processCreator;

        /// <summary>
        /// Initialises the CommandRunner with the ICommandPipeHandler to be used.
        /// </summary>
        /// <param name="pipedProcessRunner"></param>
        /// <param name="processPipeHandler"></param>
        /// <param name="processCreator"></param>
        public CommandRunner(IPipedProcessRunner pipedProcessRunner, IProcessPipeHandler processPipeHandler, IProcessCreator processCreator)
        {
            _pipedProcessRunner = pipedProcessRunner;
            _processPipeHandler = processPipeHandler;
            _processCreator = processCreator;
        }
        
        
        /// <summary>
        /// Executes a command asynchronously and returns Command execution information as a CommandResult.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>A ProcessResult object containing the execution information of the command.</returns>
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
            Process process = _processCreator.CreateProcess(_processCreator.CreateStartInfo(command));
            
            if (command.StandardInput != null)
            {
                process.StartInfo.RedirectStandardInput = true;
                await _processPipeHandler.PipeStandardInputAsync(command.StandardInput.BaseStream, process);
            }
            
            (ProcessResult processResult, Stream standardOutput, Stream standardError) result =
                await _pipedProcessRunner.ExecuteProcessWithPipingAsync(process, ProcessResultValidation.None, command.ResourcePolicy,
                    cancellationToken);

            // Throw a CommandNotSuccessful exception if required.
            if (result.processResult.ExitCode != 0 && command.ResultValidation == ProcessResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccessfulException(result.processResult.ExitCode, command);
            }
            
            if (command.StandardOutput != null)
            {
                await result.standardOutput.CopyToAsync(command.StandardOutput.BaseStream,
                    cancellationToken);
            }
            if (command.StandardError != null)
            {
                await result.standardError.CopyToAsync(command.StandardError.BaseStream, cancellationToken);
            }
            
            return result.processResult;
        }

        /// <summary>
        /// Executes a command asynchronously and returns Command execution information and Command output as a BufferedCommandResult.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="cancellationToken">A token to cancel the operation if required.</param>
        /// <returns>A BufferedProcessResult object containing the output of the command.</returns>
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
        public async Task<BufferedProcessResult> ExecuteBufferedAsync(Command command,
            CancellationToken cancellationToken = default)
        {
            Process process = _processCreator.CreateProcess(_processCreator.CreateStartInfo(command,
                true, true));

            if (command.StandardInput != null && command.StandardInput != StreamWriter.Null)
            {
                process.StartInfo.RedirectStandardInput = true;
                await _processPipeHandler.PipeStandardInputAsync(command.StandardInput.BaseStream, process);
            }
            
            // PipedProcessRunner runs the Process for us.
            (BufferedProcessResult processResult, Stream standardOutput, Stream standardError) result =
                await _pipedProcessRunner.ExecuteBufferedProcessWithPipingAsync(process, ProcessResultValidation.None, command.ResourcePolicy,
                    cancellationToken);

            // Throw a CommandNotSuccessful exception if required.
            if (result.processResult.ExitCode != 0 && command.ResultValidation == ProcessResultValidation.ExitCodeZero)
            {
                throw new CommandNotSuccessfulException(result.processResult.ExitCode, command);
            }
            
            if (command.StandardOutput != null)
            {
                await result.standardOutput.CopyToAsync(command.StandardOutput.BaseStream,
                    cancellationToken);
            }
            if (command.StandardError != null)
            {
                await result.standardError.CopyToAsync(command.StandardError.BaseStream, cancellationToken);
            }
            
            return result.processResult;
        }
}