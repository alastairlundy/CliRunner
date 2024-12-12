/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap CommandResult.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;

// ReSharper disable MemberCanBePrivate.Global

namespace CliRunner.Commands
{
    public class CommandResult
    {
        public CommandResult(int exitCode,
            DateTime startTime, DateTime exitTime)
        {
            this.ExitCode = exitCode;
            
            this.ExitTime = exitTime;
            this.StartTime = startTime;
        }
            
        public bool WasSuccessful => ExitCode == 0;
        public int ExitCode { get; }
        public DateTime StartTime { get; }
        public DateTime ExitTime { get; }

        public TimeSpan RuntimeDuration => ExitTime.Subtract(StartTime);
    }
}
