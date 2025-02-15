/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap CommandResult.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
// ReSharper disable MemberCanBeProtected.Global

// ReSharper disable MemberCanBePrivate.Global

namespace CliRunner
{
    /// <summary>
    /// A class that represents the results from an executed Process or Command.
    /// </summary>
    public class ProcessResult(
        string executableFilePath,
        int exitCode,
        DateTime startTime,
        DateTime exitTime)
    {
        /// <summary>
        /// Whether the Command successfully exited.
        /// </summary>
        public bool WasSuccessful => ExitCode == 0;
        /// <summary>
        /// The exit code from the Command that was executed.
        /// </summary>
        public int ExitCode { get; } = exitCode;

        /// <summary>
        /// 
        /// </summary>
        public string ExecutedFilePath { get; } = executableFilePath;

        /// <summary>
        /// The Date and Time that the Command's execution started.
        /// </summary>
        public DateTime StartTime { get; } = startTime;

        /// <summary>
        /// The Date and Time that the Command's execution finished.
        /// </summary>
        public DateTime ExitTime { get; } = exitTime;

        /// <summary>
        /// How long the Command took to execute represented as a TimeSpan.
        /// </summary>
        public TimeSpan RuntimeDuration => ExitTime.Subtract(StartTime);
    }
}
