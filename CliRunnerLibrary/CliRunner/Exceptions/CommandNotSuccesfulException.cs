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
    public sealed class CommandNotSuccesfulException : Exception
    {
#if NET5_0_OR_GREATER
        public Command? ExecutedCommand { get; protected set; }
#endif
        
        public int ExitCode { get; protected set; }
        
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
        /// 
        /// </summary>
        /// <param name="exitCode"></param>
        /// <param name="command"></param>
        public CommandNotSuccesfulException(int exitCode, Command command) : base(Resources.Exceptions_CommandNotSuccessful_Generic.Replace("{y}", exitCode.ToString()
            .Replace("{x}", command.TargetFilePath)))
        {
#if NET5_0_OR_GREATER
            ExecutedCommand = command;
#endif
            
            ExitCode = exitCode;
        }
    }
}