/*
    CliRunner Specializations
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensibility;
using CliRunner.Extensions;

using CliRunner.Specializations.Internal.Localizations;
// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#endif

namespace CliRunner.Specializations
{
    /// <summary>
    /// A class to make running commands through Windows Powershell easier.
    /// </summary>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("watchos")]
#endif
    public class ClassicPowershellCommand : AbstractInstallableCommand
    {
        private readonly ICommandRunner _commandRunner;
        
        /// <summary>
        /// The target file path of Windows Powershell.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Windows based operating system.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        // ReSharper disable once MemberCanBePrivate.Global
        public new string TargetFilePath
        {
            get
            {
               Task<string> task = GetInstallLocationAsync();
               task.RunSynchronously();
                       
               return task.Result + Path.DirectorySeparatorChar + "powershell.exe";
            }
        }
        
        /// <summary>
        /// Sets up ClassicPowershellCommand.
        /// </summary>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public ClassicPowershellCommand() : base("")
        {
            base.TargetFilePath = TargetFilePath;
            _commandRunner = new CommandRunner(new CommandPipeHandler());
        }

        /// <summary>
        /// Sets up ClassicPowershellCommand.
        /// </summary>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public ClassicPowershellCommand(ICommandRunner commandRunner) : base("")
        {
            base.TargetFilePath = TargetFilePath;
            _commandRunner = commandRunner;
        }

        /// <summary>
        /// Creates a new instance of the ClassicPowershellCommand class.
        /// </summary>
        /// <returns>The new ClassicPowershellCommand instance.</returns>
        [Pure]
        public static ClassicPowershellCommand CreateInstance()
        {
            return new ClassicPowershellCommand();
        }

        /// <summary>
        /// Creates a new instance of the ClassicPowershellCommand class.
        /// </summary>
        /// <returns>The new ClassicPowershellCommand instance.</returns>
        /// <param name="commandRunner">The command runner to be used for getting information about this Specialized Command.</param>
        [Pure]
        public static ClassicPowershellCommand CreateInstance(ICommandRunner commandRunner)
        {
            return new ClassicPowershellCommand(commandRunner);
        }

        
        /// <summary>
        /// Gets the installation location for Windows Powershell.
        /// </summary>
        /// <returns>the file path of where Windows Powershell is installed to.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an Operating System that is not Windows based.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public override async Task<string> GetInstallLocationAsync()
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_ClassicPowershell_OnlySupportedOnWindows);
            }

            return await Task.FromResult($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}" +
                                   $"System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0");
        }
        
        /// <summary>
        /// Detects whether Windows Powershell is installed on a system.
        /// </summary>
        /// <returns>True if running on Windows and Windows Powershell is installed; returns false otherwise.</returns>
        public new async Task<bool> IsInstalledAsync()
        {
            return OperatingSystem.IsWindows() == true && await base.IsInstalledAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsCurrentOperatingSystemSupported()
        {
            return OperatingSystem.IsWindows() == true;
        }

        /// <summary>
        /// Gets the installed version of Windows Powershell.
        /// </summary>
        /// <returns>the installed version of Windows Powershell.</returns>
        /// <exception cref="Exception">Thrown if the installed version of Windows Powershell could not be detected.</exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an Operating System that is not Windows based.</exception>
        /// <exception cref="ArgumentException">Thrown if WindowsPowershell is not installed on the system.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public override async Task<Version> GetInstalledVersionAsync()
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_ClassicPowershell_OnlySupportedOnWindows);
            }
            
            if (OperatingSystem.IsWindows() && await IsInstalledAsync())
            {
                BufferedCommandResult result = await Command.CreateInstance(this)
                    .WithArguments("$PSVersionTable")
                    .ExecuteBufferedAsync(_commandRunner);
               
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

                throw new Exception(Resources.Exceptions_Powershell_VersionNotFound);
            }

            throw new ArgumentException(Resources.Exceptions_ClassicPowershell_NotInstalled);
        }
    }
}