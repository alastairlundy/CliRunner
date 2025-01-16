/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
    
     Method signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System.Threading;
using System.Threading.Tasks;
using CliRunner.Abstractions;

using CliRunner.Buffered;

namespace CliRunner.Extensions;

public static class CommandExecuteExtensions
{
    /// <summary>
    /// Executes a command asynchronously and returns Command execution information as a CommandResult.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="commandRunner">The command runner to be used to run the command.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <returns>A CommandResult object containing the execution information of the command.</returns>
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
    public static async Task<CommandResult> ExecuteAsync(this ICommand command, ICommandRunner commandRunner,  CancellationToken cancellationToken = default)
    {
        return await commandRunner.ExecuteAsync(command, cancellationToken);
    }

    /// <summary>
    /// Executes a command asynchronously and returns Command execution information and Command output as a BufferedCommandResult.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    /// <param name="cancellationToken">A token to cancel the operation if required.</param>
    /// <param name="commandRunner">The command runner to be used to run the command.</param>
    /// <returns>A BufferedCommandResult object containing the output of the command.</returns>
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
    public static async Task<BufferedCommandResult> ExecuteBufferedAsync(this Command command, ICommandRunner commandRunner, CancellationToken cancellationToken = default)
    {
       return await commandRunner.ExecuteBufferedAsync(command, cancellationToken);
    }
}