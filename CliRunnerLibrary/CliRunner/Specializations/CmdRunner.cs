/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CliRunner.Commands;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using CliRunner.Processes;
using CliRunner.Processes.Abstractions;
using CliRunner.Specializations.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Specializations
{
    /// <summary>
    /// A class to make running commands through Windows CMD easier.
    /// </summary>
    public class CmdRunner : IRunner
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
                IEnumerable<string> args;

                if (command.Contains("cmd"))
                {
                    args = command.Replace("cmd", string.Empty).Split(' ');
                }
                else
                {
                    args = command.Split(' ');
                }
                
                return processRunner.RunProcessOnWindows(Environment.SystemDirectory,
                    "cmd", command, null, runAsAdministrator);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public string GetInstallLocation()
        {
            if (IsInstalled() == false)
            {
                throw new ArgumentException("cmd is not installed on this system.");
            }
            else
            {
                return Environment.SystemDirectory + Path.DirectorySeparatorChar + "cmd.exe";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsInstalled()
        {
            if (OperatingSystem.IsWindows())
            {
                if (File.Exists(Environment.SystemDirectory + Path.DirectorySeparatorChar + "cmd.exe"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Version GetInstalledVersion()
        {
            if (OperatingSystem.IsWindows())
            {
                ProcessResult result = Execute("--version", false);
                
                string versionString = result.StandardOutput.Split(Environment.NewLine)[0]
                    .Replace("Microsoft Windows [", string.Empty)
                    .Replace("]", string.Empty)
                    .Replace("Version",string.Empty)
                    .Replace(" ", string.Empty);

                return Version.Parse(versionString);
            }
            else
            {
                throw new PlatformNotSupportedException("cmd is not supported on Operating systems that are not based on Windows.");
            }
        }
    }
}