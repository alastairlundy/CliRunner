/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

using CliRunner.Processes;
using CliRunner.Processes.Abstractions;

using CliRunner.Specializations.Abstractions;


#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Specializations
{
    public class ClassicPowershellRunner : IRunner
    {
        protected IProcessRunner processRunner;

        public ClassicPowershellRunner()
        {
            processRunner = new ProcessRunner();
        }

        public ClassicPowershellRunner(IProcessRunner processRunner)
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
                return processRunner.RunProcessOnWindows(GetInstallLocation(),
                    "powershell", command, null, 
                    runAsAdministrator);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public string GetInstallLocation()
        {
            if (OperatingSystem.IsWindows())
            {
                return $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}" +
                       $"System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0";
            }
            else
            {
                throw new PlatformNotSupportedException();
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
                string installLocation = GetInstallLocation();

                if (Directory.Exists(installLocation))
                {
                    return Directory.GetFiles(installLocation).Contains("powershell.exe");
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public Version GetInstalledVersion()
        {
            if (OperatingSystem.IsWindows())
            {
                ProcessResult result = Execute("$PSVersionTable", false);
               
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
                string[] lines = result.StandardOutput.Split(Environment.NewLine);
#elif NETSTANDARD2_0
                string[] lines = result.StandardOutput.Split(Environment.NewLine.ToCharArray());
#endif

                foreach (string line in lines)
                {
                    if (line.ToLower().Contains("psversion"))
                    {
                        string version = line.Split(' ')[1];
                        
                        return Version.Parse(version);
                    }
                }

                throw new Exception("Failed to get psversion");
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}