/*
   Based on Tyrrrz's CliWrap CommandResult.cs
   https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Cli.cs
   
 */

using CliRunner.Commands;

namespace CliRunner
{
    public static class Cli
    {
        public static Command Run(string targetFilePath) => new Command(targetFilePath);
        
        public static Command Run(Command command) => new Command(command.TargetFilePath, command.Arguments,
            command.WorkingDirectoryPath, command.RunAsAdministrator, command.EnvironmentVariables,
            command.Credentials, command.CommandResultValidation, command.StandardInputPipe,
            command.StandardOutputPipe, command.StandardErrorPipe);
    }
}