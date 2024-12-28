/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace CliRunner.Commands.Extensions
{
    public static class CommandRunExtensions
    {
        /// <summary>
        /// Creates a Command object with the target file path. 
        /// </summary>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as <code>.WithArguments("[your args]")</code> and <code>.WithWorkingDirectory("[your directory]")</code>.</para>
        /// <para>Don't forget to call <code>.ExecuteAsync();</code> or <code>.ExecuteBufferedAsync();</code> when you're ready to execute the Command!</para>
        /// </remarks>
        /// <param name="command">The command class to instantiate.</param>
        /// <returns>A new Command object with the configured Target File Path.</returns>
        public static Command Run(this Command command)
        {
            return command;
        }
        
        /// <summary>
        /// Creates a Command object with the specified target file path. 
        /// </summary>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as <code>.WithArguments("[your args]")</code> and <code>.WithWorkingDirectory("[your directory]")</code>.</para>
        /// <para>Don't forget to call <code>.ExecuteAsync();</code> or <code>.ExecuteBufferedAsync();</code> when you're ready to execute the Command!</para>
        /// </remarks>
        /// <param name="command">The command class to instantiate.</param>
        /// <param name="targetFilePath">The target file path of the executable to wrap.</param>
        /// <returns>A new Command object with the configured Target File Path.</returns>
        public static Command Run(this Command command, string targetFilePath)
        {
            return new Command(targetFilePath);
        }
    }
}