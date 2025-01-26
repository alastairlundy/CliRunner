/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

     Method signatures from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System.Diagnostics;
using System.Threading.Tasks;
using CliRunner.Abstractions;

namespace CliRunner;

public class CommandPipeHandler : ICommandPipeHandler
{
        /// <summary>
        /// Asynchronously copies the process' Standard Input to the Command's Standard Input.
        /// </summary>
        /// <param name="source">The command to be copied from.</param>
        /// <param name="destination">The process to be copied to.</param>
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
        public async Task PipeStandardInputAsync(Command source, Process destination)
        {
            await destination.StandardInput.FlushAsync();
            destination.StandardInput.BaseStream.Position = 0;
            await source.StandardInput.BaseStream.CopyToAsync(destination.StandardInput.BaseStream);
        }

        /// <summary>
        /// Asynchronously copies the process' Standard Output to the Command's Standard Output.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination">The command to have Standard Output copied to."></param>
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
        public async Task PipeStandardOutputAsync(Process source, Command destination)
        {
            destination.StandardOutput.DiscardBufferedData();
            destination.StandardOutput.BaseStream.Position = 0;
            await source.StandardOutput.BaseStream.CopyToAsync(destination.StandardOutput.BaseStream);
        }

        /// <summary>
        /// Asynchronously copies the process' Standard Error to the Command's Standard Error.
        /// </summary>
        /// <param name="source">The process to be copied from.</param>
        /// <param name="destination">The command to have Standard Error copied to."></param>
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
        public async Task PipeStandardErrorAsync(Process source, Command destination)
        {
            destination.StandardError.DiscardBufferedData();
            destination.StandardError.BaseStream.Position = 0;
            await source.StandardError.BaseStream.CopyToAsync(destination.StandardError.BaseStream);
        }
}