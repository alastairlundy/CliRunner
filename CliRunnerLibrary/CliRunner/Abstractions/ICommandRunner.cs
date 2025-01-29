/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CliRunner.Abstractions;

/// <summary>
/// An interface to specify required Command Running functionality.
/// </summary>
public interface ICommandRunner
{
    Process CreateProcess(ProcessStartInfo processStartInfo, IntPtr processorAffinity = default);

    ProcessStartInfo CreateStartInfo(Command command);

    ProcessStartInfo CreateStartInfo(Command command, bool redirectStandardOutput, bool redirectStandardError);
        
    Task<CommandResult> ExecuteAsync(Command command, CancellationToken cancellationToken = default);
        
    Task<BufferedCommandResult> ExecuteBufferedAsync(Command command, CancellationToken cancellationToken = default);
}