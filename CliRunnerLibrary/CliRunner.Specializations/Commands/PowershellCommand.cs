/*
    CliRunner Specializations
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensibility;
using CliRunner.Extensions;
using CliRunner.Specializations.Internal.Localizations;
// ReSharper disable RedundantBoolCompare

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
// ReSharper disable MemberCanBePrivate.Global
#endif

#if NETSTANDARD2_0 || NETSTANDARD2_1
 using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Specializations
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
    public class PowershellCommand : AbstractSpecializedCommand
    {
        private readonly ICommandRunner _commandRunner;
        
        /// <summary>
        /// The target file path of cross-platform Powershell.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system besides Windows, macOS, Linux, and FreeBSD.</exception>
        public new string TargetFilePath
        {
            get
            {
                Task<string> task = GetInstallLocationAsync();
                task.RunSynchronously();

                string filePath = task.Result;

                if (OperatingSystem.IsWindows())
                {
                    filePath = $"{filePath}{Path.DirectorySeparatorChar}pwsh.exe";
                }
                else if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                {
                    filePath = $"{filePath}{Path.DirectorySeparatorChar}pwsh";
                }

                return filePath;
            }
        }

        /// <summary>
        /// Sets up the PowershellCommand class.
        /// </summary>
        public PowershellCommand() : base("")
        {
            base.TargetFilePath = TargetFilePath;
            _commandRunner = new CommandRunner(new CommandPipeHandler());
        }

        public PowershellCommand(ICommandRunner commandRunner) : base("")
        {
            base.TargetFilePath = TargetFilePath;
            _commandRunner = commandRunner;
        }

        /// <summary>
        /// Creates a new instance of the PowershellCommand class.
        /// </summary>
        /// <returns>The new PowershellCommand instance.</returns>
        [Pure]
        public static PowershellCommand CreateInstance()
        {
            return new PowershellCommand();
        }
        
        /// <summary>
        /// Creates a new instance of the PowershellCommand class.
        /// </summary>
        /// <returns>The new PowershellCommand instance.</returns>
        /// <param name="commandRunner">The command runner to be used for getting information about this Specialized Command.</param>
        [Pure]
        public static PowershellCommand CreateInstance(ICommandRunner commandRunner)
        {
            return new PowershellCommand(commandRunner);
        }
        
        /// <summary>
        /// Gets the installation location for cross-platform Powershell.
        /// </summary>
        /// <returns>The file path of where cross-platform Powershell is installed to.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an Operating System that is not Windows, macOS, Linux, or FreeBSD.</exception>
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
         public override async Task<string> GetInstallLocationAsync()
         {
             if (await IsInstalledAsync() == false)
             {
                 throw new ArgumentException(Resources.Exceptions_Powershell_NotInstalled);
             }

             BufferedCommandResult result;
             
             if (OperatingSystem.IsWindows())
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

                 result = await CmdCommand.CreateInstance(_commandRunner)
                     .WithArguments("where pwsh.exe")
                     .ExecuteBufferedAsync(_commandRunner);
             }
             else if (OperatingSystem.IsMacOS())
             {
                 result = await Command.CreateInstance("/usr/bin/which")
                     .WithArguments("pwsh")
                     .ExecuteBufferedAsync(_commandRunner);
             }
             else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
             {
                 result = await Command.CreateInstance("/usr/bin/which")
                     .WithArguments("pwsh")
                     .ExecuteBufferedAsync(_commandRunner);
             }
             else
             {
                 throw new PlatformNotSupportedException(Resources.Exceptions_Powershell_OnlySupportedOnDesktop);
             }
             
             return result.StandardOutput.Split(Environment.NewLine.ToCharArray()).First();
         }
        
        /// <summary>
        /// Detects whether cross-platform modern Powershell is installed on a system.
        /// </summary>
        /// <returns>True if cross-platform Powershell is installed; returns false otherwise.</returns>
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
        public new async Task<bool> IsInstalledAsync()
        {
            return await base.IsInstalledAsync();
        }

        /// <summary>
        /// Gets the installed version of cross-platform Powershell.
        /// </summary>
        /// <returns>The installed version of cross-platform Powershell.</returns>
        /// <exception cref="Exception">Thrown if the installed version of cross-platform Powershell could not be detected.</exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an Operating System that is not Windows, macOS, Linux, or FreeBSD.</exception>
        /// <exception cref="ArgumentException">Thrown if cross-platform Powershell is not installed on the system.</exception>
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
        public override async Task<Version> GetInstalledVersionAsync()
        {
            if (OperatingSystem.IsTvOS() || OperatingSystem.IsWatchOS() || OperatingSystem.IsAndroid() ||
                OperatingSystem.IsIOS())
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_Powershell_OnlySupportedOnDesktop);
            }
            
            BufferedCommandResult result = await Command.CreateInstance(this)
                .WithArguments("$PSVersionTable")
                .ExecuteBufferedAsync(_commandRunner);
             
            string[] lines = result.StandardOutput.Split(Environment.NewLine.ToCharArray());

            foreach (string line in lines)
            {
                if (line.ToLower().Contains("psversion"))
                {
                    string version = line.Split(' ')[1];
                         
                    return Version.Parse(version);
                }
            }

            throw new Exception(Resources.Exceptions_Powershell_VersionNotFound);
        }
    }
}