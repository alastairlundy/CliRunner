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
     public class PowershellRunner : IRunner
     {
         protected IProcessRunner processRunner;
         protected CmdRunner cmdRunner;
         public PowershellRunner()
         {
             processRunner = new ProcessRunner();
             cmdRunner = new CmdRunner();
         }
         
         /// <summary>
         /// 
         /// </summary>
         /// <param name="command"></param>
         /// <param name="runAsAdministrator"></param>
         /// <returns></returns>
         /// <exception cref="PlatformNotSupportedException"></exception>
         /// <exception cref="ArgumentException"></exception>
         public ProcessResult Execute(string command, bool runAsAdministrator)
         {
             return processRunner.RunProcessOnWindows(GetInstallLocation(),
                 "pwsh", command, null, 
                 runAsAdministrator);
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

                 ProcessResult result = cmdRunner.Execute(Environment.SystemDirectory + Path.DirectorySeparatorChar + "where pwsh.exe", false);
                 
                 if (result.StandardOutput.Split(Environment.NewLine.ToCharArray()).Any())
                 {
                     return result.StandardOutput.Split(Environment.NewLine.ToCharArray()).First();
                 }
                 
                 throw new Exception("Could not find pwsh.exe");
             }
             else if (OperatingSystem.IsMacOS())
             {
                 ProcessResult result = processRunner.RunProcessOnMac("/usr/bin", "which", "pwsh");
                 
                 return result.StandardOutput.Split(Environment.NewLine.ToCharArray())[0];
             }
             else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
             {
                 ProcessResult result = _processRunner.RunProcessOnLinux("/usr/bin", "which", new []{"pwsh"});
                 
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
                 var result = GetInstallLocation();
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
             ProcessResult result = Execute("$PSVersionTable", false);

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