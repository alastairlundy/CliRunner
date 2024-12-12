/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Threading.Tasks;
using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandPipeHandler
    {
        public async Task PipeStandardInputAsync(Process process)
        {
            await StandardInput.FlushAsync();
            StandardInput.BaseStream.Position = 0;
            await process.StandardInput.BaseStream.CopyToAsync(StandardInput.BaseStream);
        }

        public async Task PipeStandardOutputAsync(Process process)
        {
            StandardOutput.DiscardBufferedData();
            StandardOutput.BaseStream.Position = 0;
            await process.StandardOutput.BaseStream.CopyToAsync(StandardOutput.BaseStream);
        }

        public async Task PipeStandardErrorAsync(Process process)
        {
            StandardError.DiscardBufferedData();
            StandardError.BaseStream.Position = 0;
            await process.StandardError.BaseStream.CopyToAsync(StandardError.BaseStream);
        }
    }
}