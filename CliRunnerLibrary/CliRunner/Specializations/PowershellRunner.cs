/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.IO;
using System.Runtime.Versioning;

using CliRunner.Processes;
using CliRunner.Processes.Abstractions;
using CliRunner.Specializations.Abstractions;

namespace CliRunner.Specializations
{
    public class PowershellRunner : IPowershellRunner
    {
        protected IProcessRunner processRunner;

        public PowershellRunner()
        {
            processRunner = new ProcessRunner();
        }
        
        public ProcessResult Execute(string command, bool runAsAdministrator)
        {
            return processRunner.RunProcessOnWindows(GetInstallLocation(),
                "pwsh", command, null, 
                runAsAdministrator);
        }

        public string GetInstallLocation()
        {
            if (IsInstalled() == false)
            {
                throw new ArgumentException("Powershell is not installed");
            }
            
            if (OperatingSystem.IsWindows())
            {
                string programFiles;

                if (Environment.Is64BitOperatingSystem == true)
                {
                    programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }
                else
                {
                    programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                }
                
                string[] directories = Directory.GetDirectories(programFiles + Path.DirectorySeparatorChar + "Powershell");

                foreach (string directory in directories)
                {
                    if (File.Exists(directory + Path.DirectorySeparatorChar + "pwsh.exe"))
                    {
                        return directory;
                    }
                }
                
                throw new Exception("Could not find pwsh.exe");
            }
            else if (OperatingSystem.IsMacOS())
            {
                
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                ProcessResult result = processRunner.RunProcessOnLinux("/usr/bin", "which", " powershell");
                
                return res
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public bool IsInstalled()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public Version GetInstalledVersion()
        {
            ProcessResult result = Execute("$PSVersionTable", false);

            if (OperatingSystem.IsTvOS() || OperatingSystem.IsWatchOS() || OperatingSystem.IsAndroid() ||
                OperatingSystem.IsIOS())
            {
                throw new PlatformNotSupportedException();
            }
            
            string[] lines = result.StandardOutput.Split(Environment.NewLine);

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
    }
}