/*
   Based on Tyrrrz's CliWrap BufferedCommandResult.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Buffered/BufferedCommandResult.cs

   Portions of this code are licensed under the MIT license.

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