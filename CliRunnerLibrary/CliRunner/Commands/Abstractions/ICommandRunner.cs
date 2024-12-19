/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CliRunner.Commands.Buffered;

namespace CliRunner.Commands.Abstractions
{
    public interface ICommandRunner
    {
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
#endif
        Process CreateProcess(ProcessStartInfo processStartInfo);
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
#endif
        ProcessStartInfo CreateStartInfo(bool redirectStandardInput, bool redirectStandardOutput, bool redirectStandardError, Encoding encoding = default);
        
        Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);
        
        Task<BufferedCommandResult> ExecuteBufferedAsync(CancellationToken cancellationToken = default);
        Task<BufferedCommandResult> ExecuteBufferedAsync(Encoding encoding, CancellationToken cancellationToken = default);


    }
}