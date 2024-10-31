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
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace CliRunner.Specializations
{
    public class CmdRunner : ICmdRunner
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