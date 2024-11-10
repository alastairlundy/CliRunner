/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Runtime.Versioning;

using CliRunner.Processes;
using CliRunner.Processes.Abstractions;
using CliRunner.Specializations.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Specializations
{
    public class CmdRunner
    /// <summary>
    /// A class to make running commands through Windows CMD easier.
    /// </summary>
    {
        protected IProcessRunner processRunner;

        public CmdRunner()
        {
            processRunner = new ProcessRunner();
        }

        public CmdRunner(IProcessRunner processRunner)
        {
            this.processRunner = processRunner;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">The command to run.</param>
        /// <param name="runCmdAsAdministrator">Whether to run CMD as an administrator.</param>
        /// <returns>the result of running the command via CMD.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on an Operating System based on Windows.</exception>
        #if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        #endif
        public ProcessResult Execute(string command, bool runAsAdministrator)
        {
            if (OperatingSystem.IsWindows())
            {
                return processRunner.RunProcessOnWindows(Environment.SystemDirectory,
                    "cmd", command, null, runAsAdministrator);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}