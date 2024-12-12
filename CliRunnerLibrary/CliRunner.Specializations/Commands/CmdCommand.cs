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
using System.Threading.Tasks;
using CliRunner.Commands;

using CliRunner.Extensibility;

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
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("watchos")]
#endif
    public class CmdCommand : Command, ISpecializedCommandInformation
    {
        public new string TargetFilePath => GetInstallLocationAsync().Result;
        
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
        public CmdCommand() : base(targetFilePath: "")
        {
            base.TargetFilePath = TargetFilePath;
        }

        public static CmdCommand Create()
        {
            return new CmdCommand();
        }
        
        public async Task<string> GetInstallLocationAsync()
        {
            if (await IsInstalledAsync() == false)
            {
                throw new ArgumentException("cmd.exe is not installed on this system. This may be because you are running a non-windows operating system.");
            }
            else
            {
                return await Task.FromResult(Environment.SystemDirectory + Path.DirectorySeparatorChar + "cmd.exe");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsInstalledAsync()
        {
            if (OperatingSystem.IsWindows())
            {
                return Task.FromResult(File.Exists(Environment.SystemDirectory + Path.DirectorySeparatorChar + "cmd.exe"));
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public async Task<Version> GetInstalledVersionAsync()
        {
            if (OperatingSystem.IsWindows())
            {
               var result =  await CliRunner.Wrap(this)
                    .WithArguments("--version")
                    .WithWorkingDirectory(Environment.SystemDirectory)
                    .RequiresAdministrator(false)
                    .ExecuteBufferedAsync();

#if NET5_0_OR_GREATER
                string output = result.StandardOutput.Split(Environment.NewLine)[0]
                    .Replace("Microsoft Windows [", string.Empty)
                    .Replace("]", string.Empty)
                    .Replace("Version",string.Empty)
                    .Replace(" ", string.Empty);
#else
                string versionString = result.StandardOutput
                    .Replace("Microsoft Windows [", string.Empty)
                    .Replace("]", string.Empty)
                    .Replace("Version", string.Empty);

                string output = versionString.Split(' ').First();
#endif
                   
                return Version.Parse(output);
            }
            else
            {
                throw new PlatformNotSupportedException("cmd is not supported on Operating systems that are not based on Windows.");
            }
        }
    }
}