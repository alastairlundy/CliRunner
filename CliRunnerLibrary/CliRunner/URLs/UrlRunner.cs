/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System.Threading.Tasks;

using CliRunner.Commands;
using CliRunner.Commands.Extensions;

using CliRunner.Urls.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Urls
{

    public partial class Url : IUrlRunner
    {


        /// <summary>
        /// Asynchronously opens the Url in the default browser.
        /// </summary>
        /// <remarks>Some code contained in this method is courtesy of https://github.com/dotnet/corefx/issues/10361</remarks>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on a platform besides Windows, macOS, FreeBSD, or Linux.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
#endif
        public async Task<UrlResult> OpenWithDefaultBrowserAsync()
        {
            CommandResult result;

            string url = ToString();
            
            if (OperatingSystem.IsWindows())
            {
                string args = $"/c start {url.Replace("&", "^&")}";
                
               result = await Cli.Run($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}cmd.exe")
                    .WithArguments(args)
                    .WithWorkingDirectory(Environment.SystemDirectory)
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync();
               
               return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime, FromString(url)));
            }
            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
               result = await Cli.Run("/usr/bin/xdg-open")
                    .WithArguments(url.Replace("&", "^&"))
                    .WithWorkingDirectory("/usr/bin")
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync();
               
               return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime, FromString(url)));
            }
            if (OperatingSystem.IsMacOS())
            {
                Process process = new Process();
                
                Task task = new Task(() => process = Process.Start("open", url));
                task.Start();

                await task.ConfigureAwait(false);

                result = process.ToCommandResult();
                
                return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime, FromString(url)));
            }
            
            throw new PlatformNotSupportedException("This method is only supported on Windows, macOS, FreeBSD, or Linux.");
        }
    }        
}