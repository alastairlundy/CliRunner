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
using CliRunner.Builders;
using CliRunner.Extensibility;

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
    [Obsolete("This class is deprecated and will be removed in a future version.")]
    public class ClassicPowershellCommand : AbstractInstallableCommand
    {
        private readonly ICommandRunner _commandRunner;

        private readonly Command _psVersionCommand;
        
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
                if (OperatingSystem.IsWindows() == false)
                {
                    throw new PlatformNotSupportedException(Resources.Exceptions_ClassicPowershell_OnlySupportedOnWindows);
                }

                string location = $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}" +
                                             $"System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0";
                       
                return $"{location}{Path.DirectorySeparatorChar}powershell.exe";
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

            ICommandBuilder commandBuilder = new CommandBuilder(this)
                .WithArguments("$PSVersionTable");
            
            _psVersionCommand = commandBuilder.ToCommand();
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
            
            ICommandBuilder commandBuilder = new CommandBuilder(this)
                .WithArguments("$PSVersionTable");
            
            _psVersionCommand = commandBuilder.ToCommand();
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
            
            if (OperatingSystem.IsWindows() && IsInstalled())
            {
                BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(_psVersionCommand);
               
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