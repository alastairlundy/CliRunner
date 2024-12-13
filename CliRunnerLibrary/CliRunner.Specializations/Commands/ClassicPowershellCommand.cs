/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliRunner.Commands;

using CliRunner.Extensibility;

#if NETSTANDARD2_0 || NETSTANDARD2_1
 using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
 // ReSharper disable RedundantBoolCompare
#endif

namespace CliRunner.Specializations.Commands
{
    /// <summary>
    /// 
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
    public class ClassicPowershellCommand : Command, ISpecializedCommandInformation
    {

        public new string TargetFilePath => GetInstallLocationAsync().Result + Path.DirectorySeparatorChar + "powershell.exe";

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
        public ClassicPowershellCommand() : base("Powershell.exe")
        {
            base.TargetFilePath = TargetFilePath;
            UseShellExecute = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Command Create()
        {
            return new ClassicPowershellCommand();
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
        public Task<string> GetInstallLocationAsync()
        {
            if (OperatingSystem.IsWindows())
            {
                return Task.FromResult($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}" +
                                       $"System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0");
            }
            else
            {
                throw new PlatformNotSupportedException("WindowsPowerShell only supports Windows.");
            }
        }
        
        /// <summary>
        /// Detects whether Windows Powershell is installed on a system.
        /// </summary>
        /// <returns>true if running on Windows and Windows Powershell is installed; returns false otherwise.</returns>
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
        public async Task<bool> IsInstalledAsync()
        {
            if (OperatingSystem.IsWindows())
            {
                string installLocation = await GetInstallLocationAsync();

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
        /// Gets the installed version of Windows Powershell.
        /// </summary>
        /// <returns>the installed version of Windows Powershell.</returns>
        /// <exception cref="Exception">Thrown if the installed version of Windows Powershell could not be detected.</exception>
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
        public async Task<Version> GetInstalledVersionAsync()
        {
            if (OperatingSystem.IsWindows() && await IsInstalledAsync())
            {
                var result = await CliRunner.Wrap(this)
                    .WithArguments("$PSVersionTable")
                    .RequiresAdministrator(false)
                    .ExecuteBufferedAsync();
               
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