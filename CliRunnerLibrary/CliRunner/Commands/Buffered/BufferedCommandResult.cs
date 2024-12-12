/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap BufferedCommandResult.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Buffered/BufferedCommandResult.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;

namespace CliRunner.Commands.Buffered
{
    /// <summary>
    /// 
    /// </summary>
    public class BufferedCommandResult : CommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string StandardInput { get; }

        /// <summary>
        /// 
        /// </summary>
        public string StandardOutput { get; }
        
        public BufferedCommandResult(int exitCode, string standardInput, string standardOutput,
            DateTime startTime, DateTime exitTime)
            : base(exitCode, startTime, exitTime)
        {
            StandardInput = standardInput;
            StandardOutput = standardOutput;
        }
    }
}