/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#if NET5_0_OR_GREATER
    #nullable enable
#endif

using System;

using CliRunner.Commands;
using CliRunner.Internal.Localizations;

namespace CliRunner.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommandNotSuccessfulException : Exception
    {
#if NET5_0_OR_GREATER
        public Command? ExecutedCommand { get; private set; }
#endif
        
        public int ExitCode { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exitCode"></param>
        public CommandNotSuccesfulException(int exitCode) : base(Resources.Exceptions_CommandNotSuccessful_Generic.Replace("{x}", exitCode.ToString()))
        {
            ExitCode = exitCode;
            
#if NET5_0_OR_GREATER
            ExecutedCommand = null;
#endif
        }

        /// <summary>
        /// Thrown when a Command that was executed exited with a non-zero exit code.
        /// </summary>
        /// <param name="exitCode">The exit code of the Command that was executed.</param>
        /// <param name="command">The command that was executed.</param>
        public CommandNotSuccessfulException(int exitCode, Command command) : base(Resources.Exceptions_CommandNotSuccessful_Specific.Replace("{y}", exitCode.ToString()
            .Replace("{x}", command.TargetFilePath)))
        {
#if NET5_0_OR_GREATER
            ExecutedCommand = command;
            Source = ExecutedCommand.TargetFilePath;
#endif
            
            ExitCode = exitCode;
        }
    }
}