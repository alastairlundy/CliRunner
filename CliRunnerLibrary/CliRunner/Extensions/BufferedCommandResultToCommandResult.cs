/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using CliRunner.Buffered;

namespace CliRunner.Extensions
{
    public static class BufferedCommandResultToCommandResult
    {
        /// <summary>
        /// Converts a Buffered Command Result to a Command Result.
        /// This is a one way operation and may not be reversible.
        /// </summary>
        /// <param name="bufferedCommandResult">The buffered command result to be converted.</param>
        /// <returns>the converted Command Result.</returns>
        public static CommandResult ToCommandResult(this BufferedCommandResult bufferedCommandResult)
        {
            return new CommandResult(bufferedCommandResult.ExitCode, bufferedCommandResult.StartTime, bufferedCommandResult.ExitTime);
        }
    }
}