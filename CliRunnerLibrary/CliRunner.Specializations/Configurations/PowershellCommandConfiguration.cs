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
using CliRunner.Extensibility;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif

// ReSharper disable RedundantBoolCompare

namespace CliRunner.Specializations.Configurations
{
    /// <summary>
    /// A class to make running commands through cross-platform Powershell easier.
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
    public class PowershellCommandConfiguration : SpecializedCommandConfiguration
    {
        private readonly ICommandRunner _commandRunner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiresAdministrator"></param>
        /// <param name="workingDirectoryPath"></param>
        /// <param name="commandRunner"></param>
        /// <param name="arguments"></param>
        /// <param name="environmentVariables"></param>
        /// <param name="credentials"></param>
        /// <param name="resultValidation"></param>
        /// <param name="standardInput"></param>
        /// <param name="standardOutput"></param>
        /// <param name="standardError"></param>
        /// <param name="useShellExecution"></param>
        /// <param name="windowCreation"></param>
        /// <param name="standardInputEncoding"></param>
        /// <param name="standardOutputEncoding"></param>
        /// <param name="standardErrorEncoding"></param>
        /// <param name="processorAffinity"></param>
        public PowershellCommandConfiguration(ICommandRunner commandRunner, string arguments = null,
            string workingDirectoryPath = null, bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null, UserCredential credentials = null,
            CommandResultValidation resultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null, StreamReader standardOutput = null, StreamReader standardError = null,
            Encoding standardInputEncoding = default, Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default, IntPtr processorAffinity = default(IntPtr),
            bool useShellExecution = false, bool windowCreation = false) : base("", arguments,
            workingDirectoryPath,
            requiresAdministrator, environmentVariables, credentials, resultValidation, standardInput, standardOutput,
            standardError, standardInputEncoding, standardOutputEncoding, standardErrorEncoding, processorAffinity,
            useShellExecution, windowCreation)
        {
            _commandRunner = commandRunner;
        }
        
        /// <summary>
        /// The target file path of cross-platform Powershell.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system besides Windows, macOS, Linux, and FreeBSD.</exception>
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
        public new string TargetFilePath
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
    }
}