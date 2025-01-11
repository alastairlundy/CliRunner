/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CliRunner.Commands.Buffered;

namespace CliRunner.Commands.Abstractions
{
    /// <summary>
    /// An interface to specify required Command Running functionality.
    /// </summary>
    public interface ICommandRunner
    {
        Process CreateProcess(ProcessStartInfo processStartInfo);
        
        ProcessStartInfo CreateStartInfo(bool redirectStandardInput, bool redirectStandardOutput, bool redirectStandardError, bool createNoWindow = false, Encoding encoding = default);
        
        Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);
        Task<CommandResult> ExecuteAsync(Encoding encoding, CancellationToken cancellationToken = default);

        
        Task<BufferedCommandResult> ExecuteBufferedAsync(CancellationToken cancellationToken = default);
        Task<BufferedCommandResult> ExecuteBufferedAsync(Encoding encoding, CancellationToken cancellationToken = default);
    }
}