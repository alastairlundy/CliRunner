/*
   Based on Tyrrrz's CliWrap CommandResult.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs

   Portions of this code are licensed under the MIT license.

 */

using System;

// ReSharper disable MemberCanBePrivate.Global

namespace CliRunner.Commands
{
    public class CommandResult
    {
        public CommandResult(int exitCode, string standardOutput,
            DateTime startTime, DateTime exitTime)
        {
            this.ExitCode = exitCode;
            this.StandardOutput = standardOutput;
            
            this.ExitTime = exitTime;
            this.StartTime = startTime;
        }
            
        public bool WasSuccessful => ExitCode == 0;
        public int ExitCode { get; }
        public DateTime StartTime { get; }
        public DateTime ExitTime { get; }

        public TimeSpan RuntimeDuration => ExitTime.Subtract(StartTime);

        public string StandardOutput { get; }
    }
}
