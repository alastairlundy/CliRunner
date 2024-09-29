/*
 * Based on Tyrrrz's CliWrap CommandResult.cs
 * https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs
 * MIT License - Credit: Tyrrrz
 */

using System;
using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public class ProcessResult : IProcessResult
    {
        public ProcessResult(int exitCode, string standardOutput,
            DateTime startTime, DateTime exitTime)
        {
            this.ExitCode = exitCode;
            this.StandardOutput = standardOutput;
            
            this.ExitTime = exitTime;
            this.StartTime = startTime;
        }
        
        public int ExitCode { get; }

        public bool WasSuccessful => ExitCode == 0;
        
        public string StandardOutput { get; }
        
        public DateTime StartTime { get; }
        public DateTime ExitTime { get; }

        public TimeSpan RuntimeDuration => ExitTime.Subtract(StartTime);
    }
}