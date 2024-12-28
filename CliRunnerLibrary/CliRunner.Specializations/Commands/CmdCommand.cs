/*
    CliRunner Specializations
    Copyright (C) 2024  Alastair Lundy

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
using System.Linq;
using System.Threading.Tasks;

using CliRunner.Commands.Buffered;
using CliRunner.Extensibility;

using CliRunner.Specializations.Internal.Localizations;
// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
 using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
 // ReSharper disable RedundantBoolCompare

 using System.Linq;
#endif

namespace CliRunner.Specializations.Commands
{
    /// <summary>
    /// A class to make running commands through Windows CMD easier.
    /// </summary>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("watchos")]
#endif
    public class CmdCommand : AbstractSpecializedCommand
    {
        /// <summary>
        /// The target file path of Cmd.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Windows based operating system.</exception>
        public new string TargetFilePath
        {
            get
            {
                Task<string> task = GetInstallLocationAsync();
                task.RunSynchronously();

                return $"{task.Result}{Path.DirectorySeparatorChar}cmd.exe";
            }
        }
        
        /// <summary>
        /// Sets up the CmdCommand class.
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
        public CmdCommand() : base("")
        {
            base.TargetFilePath = TargetFilePath;
        }

        /// <summary>
        /// Creates a new instance of the CmdCommand class.
        /// </summary>
        /// <returns>The new CmdCommand instance.</returns>
        [Pure]
        public static CmdCommand Run()
        {
            return new CmdCommand();
        }
        
        /// <summary>
        /// Asynchronously gets the installation location of CMD, if it is installed.
        /// </summary>
        /// <returns>The file path where CMD is installed if run on Windows.</returns>
        /// <exception cref="ArgumentException">Thrown if CMD is not found on the current system.</exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if CMD is run on a non Windows based operating system.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("browser")]
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
                throw new PlatformNotSupportedException(Resources.Exceptions_Cmd_OnlySupportedOnWindows);
            }
            
            if (await IsInstalledAsync() == false)
            {
                throw new ArgumentException(Resources.Exceptions_Cmd_NotInstalled);
            }

            return await Task.FromResult($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}cmd.exe");
        }

        /// <summary>
        /// Asynchronously returns whether CMD is installed on the current system.
        /// </summary>
        /// <returns>A task that returns true if cmd.exe exists on Windows; returns false otherwise.</returns>
        public new Task<bool> IsInstalledAsync()
        {
            if (OperatingSystem.IsWindows() == false)
            {
                return Task.FromResult(false);
            }
            
            return Task.FromResult(File.Exists($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}cmd.exe"));
        }

        /// <summary>
        /// Asynchronously returns the installed version of CMD if the current OS is Windows.
        /// </summary>
        /// <returns>The installed version of CMD if the current operating system is Windows based.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an operating system that isn't based on Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("browser")]
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
                throw new PlatformNotSupportedException(Resources.Exceptions_Cmd_NotInstalled);
            }
            
            BufferedCommandResult result =  await Cli.Run(this)
                .WithArguments("--version")
                .WithWorkingDirectory(Environment.SystemDirectory)
                .ExecuteBufferedAsync();

#if NET5_0_OR_GREATER
            string output = result.StandardOutput.Split(Environment.NewLine).First()
                .Replace("Microsoft Windows [", string.Empty)
                .Replace("]", string.Empty)
                .Replace("Version",string.Empty)
                .Replace(" ", string.Empty);
#else
                string output = result.StandardOutput
                    .Replace("Microsoft Windows [", string.Empty)
                    .Replace("]", string.Empty)
                    .Replace("Version", string.Empty).Split(' ').First();
#endif
                   
            return Version.Parse(output);
        }
    }
}