/*
 * Based on Tyrrrz's CliWrap CommandResult.cs
 * https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs
 * MIT License - Credit: Tyrrrz
 */

using System;

namespace CliRunner.Processes.Abstractions
{
    public interface IProcessResult
    {
        int ExitCode { get; }
        
        bool WasSuccessful { get; }
        
        string StandardOutput { get; }
        
        DateTime StartTime { get; }
        DateTime ExitTime { get; }

        TimeSpan RuntimeDuration { get; }
    }
}