/*
   Based on Tyrrrz's CliWrap ICommandCongifuration.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/ICommandConfiguration.cs

    Portions of this code are licensed under the MIT license.
 */

using System.Collections.Generic;
using CliRunner.Piping.Abstractions;

namespace CliRunner.Commands.Abstractions
{
    public interface ICommandConfiguration
    {
        bool RunAsAdministrator { get; }
        string TargetFilePath { get; }
        string WorkingDirectoryPath { get; }

        string Arguments { get; }

        IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        CliRunner.Credentials Credentials { get;  } 
        CliRunner.Commands.CommandResultValidation CommandResultValidation { get;}

        AbstractPipeSource StandardInputPipe { get; }
        AbstractPipeTarget StandardOutputPipe { get; }
        AbstractPipeTarget StandardErrorPipe { get; }
    }
}