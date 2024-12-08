﻿/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Diagnostics;
using System.Threading.Tasks;

using CliRunner.Piping;

namespace CliRunner.Commands.Abstractions
{
    public interface ICommandPipeHandler
    {
        Task<PipeSource> PipeStandardInputAsync(Process process);
        Task<PipeTarget> PipeStandardOutputAsync(Process process);
        Task<PipeTarget> PipeStandardErrorAsync(Process process);
    }
}