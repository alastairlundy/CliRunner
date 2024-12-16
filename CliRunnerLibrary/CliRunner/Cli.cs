/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Cli.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Cli.cs

     Method signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using CliRunner.Commands;

namespace CliRunner
{
    public static class Cli
    {
        /// <summary>
        /// Builds a Command
        /// Creates a Command object with the specified target file path. 
        /// </summary>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as WithArguments("[your args]") and WithWorkingDirectory("[your directory]").</para>
        /// <para>Don't forget to call .ExecuteAsync() or ExecuteBufferedAsync() when you're done!</para>
        /// </remarks>
        /// <param name="targetFilePath">The target file path of the executable to wrap.</param>
        /// <returns></returns>
        public static Command Run(string targetFilePath) => new Command(targetFilePath);
        
        /// <summary>
        /// Used to wrap an existing Command object when a modified version is desired.
        /// </summary>
        /// <param name="command">The command to wrap</param>
        /// <returns></returns>
        public static Command Run(Command command) => new Command(command.TargetFilePath, command.Arguments,
            command.WorkingDirectoryPath, command.RequiresAdministrator, command.EnvironmentVariables,
            command.Credentials, command.ResultValidation, command.StandardInput,
            command.StandardOutput, command.StandardError);
    }
}