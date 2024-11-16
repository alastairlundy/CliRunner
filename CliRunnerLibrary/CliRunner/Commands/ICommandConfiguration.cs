/*
 * Based on Tyrrrz's CliWrap ICommandCongifuration.cs
 * https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/ICommandConfiguration.cs

Portions of this code are licensed under the MIT license.

 */

using System.Collections.Generic;
using CliRunner.Piping.Abstractions;

namespace CliRunner.Commands
{
    public interface ICommandConfiguration
    {
        public string TargetFilePath { get; }
        public string WorkingDirectoryPath { get; }
        
        public string[] Arguments { get; }

        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
        
        public CliRunner.Credentials Credentials { get;  }
        public CliRunner.Processes.ProcessResultValidator ProcessResultValidator { get; }
         
        public AbstractPipeSource StandardInputPipe { get; }
        public AbstractPipeTarget StandardOutputPipe { get; }
        public AbstractPipeTarget StandardErrorPipe { get; }
    }
}