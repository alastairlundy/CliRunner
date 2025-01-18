/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap BufferedCommandResult.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Buffered/BufferedCommandResult.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;

namespace CliRunner.Buffered
{
    /// <summary>
    /// A buffered CommandResult containing a Command's StandardOutput and StandardError information.
    /// </summary>
    public class BufferedCommandResult(
        int exitCode,
        string standardOutput,
        string standardError,
        DateTime startTime,
        DateTime exitTime)
        : CommandResult(exitCode, startTime, exitTime)
    {
        /// <summary>
        /// The Standard Output from the Command represented as a string.
        /// </summary>
        public string StandardOutput { get; } = standardOutput;

        /// <summary>
        /// The Standard Error from the Command represented as a string.
        /// </summary>
        public string StandardError { get; } = standardError;
    }
}