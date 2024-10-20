/*
 * Based on Tyrrrz's CliWrap CommandResult.cs
 * https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/CommandResult.cs

Portions of this code are licensed under the MIT license.

--------------------------------------------------------------------------
MIT License

Copyright (c) 2017-2024 Oleksii Holub

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
 */

using System;

using CliRunner.Processes.Abstractions;

namespace CliRunner.Processes
{
    public class ProcessResult
    {
        public ProcessResult(int exitCode, string standardOutput,
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
