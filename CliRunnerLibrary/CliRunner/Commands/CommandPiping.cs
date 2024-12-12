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
using System.Threading.Tasks;
using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandPipeHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        public async Task PipeStandardInputAsync(Process process)
        {
            await StandardInput.FlushAsync();
            StandardInput.BaseStream.Position = 0;
            await process.StandardInput.BaseStream.CopyToAsync(StandardInput.BaseStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        public async Task PipeStandardOutputAsync(Process process)
        {
            StandardOutput.DiscardBufferedData();
            StandardOutput.BaseStream.Position = 0;
            await process.StandardOutput.BaseStream.CopyToAsync(StandardOutput.BaseStream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        public async Task PipeStandardErrorAsync(Process process)
        {
            StandardError.DiscardBufferedData();
            StandardError.BaseStream.Position = 0;
            await process.StandardError.BaseStream.CopyToAsync(StandardError.BaseStream);
        }
    }
}