/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CliRunner.Commands.Buffered;
// ReSharper disable RedundantBoolCompare

namespace CliRunner.Commands.Extensions
{
    public static class ProcessToCommandResultExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process">The exited process to convert to a CommandResult object.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if a non-exited process is passed as a parameter.</exception>
        public static CommandResult ToCommandResult(this Process process)
        {
            if (process.HasExited == false)
            {
                throw new ArgumentException("Cannot convert an non-exited process to a CommandResult");
            }
            
            return new CommandResult(process.ExitCode, process.StartTime, process.ExitTime);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process">The exited process to convert to a BufferedCommandResult object.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<BufferedCommandResult> ToBufferedCommandResultAsync(this Process process)
        {
            if (process.HasExited == false)
            {
                throw new ArgumentException("Cannot convert an non-exited process to a CommandResult");
            }

            if (process.StartInfo.RedirectStandardOutput == true && process.StartInfo.RedirectStandardError == true)
            {
                return new BufferedCommandResult(process.ExitCode,await process.StandardOutput.ReadToEndAsync(),
                    await process.StandardError.ReadToEndAsync(), process.StartTime, process.ExitTime);
            }
            
            throw new ArgumentException("Cannot convert a process without StandardOutput/StandardError to a BufferedCommandResult.");
        }
    }
}