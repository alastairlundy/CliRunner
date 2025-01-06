/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

     Method signatures from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System.Diagnostics;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System.Threading.Tasks;

using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandPipeHandler
    {
        /// <summary>
        /// Asynchronously copies the process' Standard Input to the Command's Standard Input.
        /// </summary>
        /// <param name="process">The process to be copied from.</param>
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
        public async Task PipeStandardInputAsync(Process process)
        {
            await StandardInput.FlushAsync();
            StandardInput.BaseStream.Position = 0;
            await process.StandardInput.BaseStream.CopyToAsync(StandardInput.BaseStream);
        }

        /// <summary>
        /// Asynchronously copies the process' Standard Output to the Command's Standard Output.
        /// </summary>
        /// <param name="process">The process to be copied from.</param>
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
        public async Task PipeStandardOutputAsync(Process process)
        {
            StandardOutput.DiscardBufferedData();
            StandardOutput.BaseStream.Position = 0;
            await process.StandardOutput.BaseStream.CopyToAsync(StandardOutput.BaseStream);
        }

        /// <summary>
        /// Asynchronously copies the process' Standard Error to the Command's Standard Error.
        /// </summary>
        /// <param name="process">The process to be copied from.</param>
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
        public async Task PipeStandardErrorAsync(Process process)
        {
            StandardError.DiscardBufferedData();
            StandardError.BaseStream.Position = 0;
            await process.StandardError.BaseStream.CopyToAsync(StandardError.BaseStream);
        }
    }
}