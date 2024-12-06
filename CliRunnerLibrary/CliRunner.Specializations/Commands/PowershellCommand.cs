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

using CliRunner.Commands;
using CliRunner.Extensibility;
// ReSharper disable RedundantBoolCompare

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

#if NETSTANDARD2_0 || NETSTANDARD2_1
 using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
 // ReSharper disable RedundantBoolCompare
#endif

namespace CliRunner.Specializations.Commands
{
    public class PowershellCommand : Command, ISpecializedCommandInformation
    {
        public PowershellCommand() : base("")
        {
            
        }
        
        /// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
         /// <exception cref="ArgumentException"></exception>
         /// <exception cref="Exception"></exception>
         /// <exception cref="PlatformNotSupportedException"></exception>
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

                 BufferedCommandResult result = Cli.Run(CmdCommand.Create())
                     .WithArguments("where pwsh.exe")
                     .RequiresAdministrator(false)
                     .ExecuteBuffered();
                 
                 if (result.StandardOutput.Split(Environment.NewLine.ToCharArray()).Any())
                 {
                     return result.StandardOutput.Split(Environment.NewLine.ToCharArray()).First();
                 }
                 
                 throw new Exception("Could not find pwsh.exe");
             }
             else if (OperatingSystem.IsMacOS())
             {
                 BufferedCommandResult result = Cli.Run("/usr/bin/which")
                     .WithArguments("pwsh")
                     .RequiresAdministrator(false)
                     .ExecuteBuffered();
                     
                 return result.StandardOutput.Split(Environment.NewLine.ToCharArray())[0];
             }
             else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
             {
                 BufferedCommandResult result = Cli.Run("/usr/bin/which")
                     .WithArguments("pwsh")
                     .RequiresAdministrator(false)
                     .ExecuteBuffered();
                 
                 return result.StandardOutput.Split(Environment.NewLine.ToCharArray())[0];
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
            try
            {
                BufferedCommandResult result;

                if (OperatingSystem.IsWindows())
                {
                     result = Cli.Run(CmdCommand.Create())
                        .WithArguments("where pwsh.exe")
                        .RequiresAdministrator(false)
                        .ExecuteBuffered();
                }
                else if (OperatingSystem.IsMacOS())
                {
                    result = Cli.Run("/usr/bin/which")
                        .WithArguments("pwsh")
                        .RequiresAdministrator(false)
                        .ExecuteBuffered();
                }
                else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                {
                    result = Cli.Run("/usr/bin/which")
                        .WithArguments("pwsh")
                        .RequiresAdministrator(false)
                        .ExecuteBuffered();
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }

                if (result.StandardOutput.ToLower().Contains("error") ||
                    result.StandardOutput.ToLower().Contains("not found"))
                {
                    return false;
                }
                 
                return true;
            }
            catch
            {
                return false;
            }
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
            BufferedCommandResult result = Cli.Run(this)
                .WithArguments("$PSVersionTable")
                .RequiresAdministrator(false)
                .ExecuteBuffered();

            if (OperatingSystem.IsTvOS() || OperatingSystem.IsWatchOS() || OperatingSystem.IsAndroid() ||
                OperatingSystem.IsIOS())
            {
                throw new PlatformNotSupportedException();
            }
             
            string[] lines = result.StandardOutput.Split(Environment.NewLine.ToCharArray());

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