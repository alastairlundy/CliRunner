/*
    CliRunner Specializations
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using CliRunner.Abstractions;
using CliRunner.Builders;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif

// ReSharper disable RedundantBoolCompare

namespace CliRunner.Specializations.Configurations
{
    /// <summary>
    /// 
    /// </summary>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("freebsd")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("watchos")]
#endif
    public class PowershellCommandConfiguration : ICommandConfiguration
    {
        private readonly ICommandRunner _commandRunner;

        public PowershellCommandConfiguration(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
        /// <summary>
        /// The target file path of cross-platform Powershell.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system besides Windows, macOS, Linux, and FreeBSD.</exception>
        public string TargetFilePath
        {
            get
            {
                string filePath = string.Empty;
                
                if (OperatingSystem.IsWindows())
                {
                    filePath = $"{GetWindowsInstallLocation()}{Path.DirectorySeparatorChar}pwsh.exe";
                }
                else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                {
                    filePath = GetUnixInstallLocation();
                }

                return filePath;
            }
        }

        private string GetWindowsInstallLocation()
        {
            string programFiles = Environment.GetFolderPath(Environment.Is64BitOperatingSystem == true ?
                Environment.SpecialFolder.ProgramFiles : Environment.SpecialFolder.ProgramFilesX86);

            string[] directories = Directory.GetDirectories(
                $"{programFiles}{Path.DirectorySeparatorChar}Powershell");

            foreach (string directory in directories)
            {
                if (File.Exists($"{directory}{Path.DirectorySeparatorChar}pwsh.exe"))
                {
                    return directory;
                }
            }
            
            throw new FileNotFoundException("Could not find Powershell installation.");
        }

        private string GetUnixInstallLocation()
        {
           ICommandBuilder installLocationBuilder = new CommandBuilder("/usr/bin/which")
                .WithArguments("pwsh");
           
           Command command = installLocationBuilder.ToCommand();
           
          Task<BufferedCommandResult> task = _commandRunner.ExecuteBufferedAsync(command);
          
          task.RunSynchronously();
          
          Task.WaitAll(task);
          
          return task.Result.StandardOutput;
        }
        
        public bool RequiresAdministrator { get; }
        public string WorkingDirectoryPath { get; }
        public string Arguments { get; }
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
        public UserCredentials Credentials { get; }
        public CommandResultValidation ResultValidation { get; }
        public StreamWriter StandardInput { get; }
        public StreamReader StandardOutput { get; }
        public StreamReader StandardError { get; }
        public bool UseShellExecution { get; }
        public bool WindowCreation { get; }
        public Encoding StandardInputEncoding { get; }
        public Encoding StandardOutputEncoding { get; }
        public Encoding StandardErrorEncoding { get; }
        public IntPtr ProcessorAffinity { get; }
    }
}