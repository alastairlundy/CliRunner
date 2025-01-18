/*
    UrlRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Commands;
using CliRunner.Commands.Extensions;

namespace UrlRunner;

public class UrlCommand
{
    public Uri Url { get; protected set; }

    public Command? BrowserOpenCommand { get; protected set; }

    public UrlCommand(Uri uri, Command? browserOpenCommand = null)
    {
        Url = uri;
        BrowserOpenCommand = browserOpenCommand;
    }
    
    public static UrlCommand Run(Uri uri)
    {
        return new UrlCommand(uri);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public UrlCommand WithBrowser(Command command)
    {
        return new UrlCommand(Url, command);
    }

    /// <summary>
    /// Asynchronously opens the Url in the default browser.
    /// </summary>
    /// <param name="uri"></param>
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
    public async Task<UrlResult> OpenInDefaultBrowserAsync()
    {
        CommandResult result;

        string url = Url.ToString();

        if (OperatingSystem.IsWindows())
        {
            string args = $"/c start {url.Replace("&", "^&")}";
                
            result = await Cli.Run($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}cmd.exe")
                .WithArguments(args)
                .WithWorkingDirectory(Environment.SystemDirectory)
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync();
               
            return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime, url));
        }
        if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
        {
            result = await Cli.Run("/usr/bin/xdg-open")
                .WithArguments(url.Replace("&", "^&"))
                .WithWorkingDirectory("/usr/bin")
                .WithValidation(CommandResultValidation.None)
                .ExecuteAsync();
               
            return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime, url));
        }
        if (OperatingSystem.IsMacOS())
        {
            Process process = new Process();
                
            Task task = new Task(() => process = Process.Start("open", url));
            task.Start();

            await task.ConfigureAwait(false);

            result = process.ToCommandResult();
                
            return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime, url));
        }

        throw new PlatformNotSupportedException("Operating not supported on " + RuntimeInformation.OSDescription);
    }

    /// <summary>
    /// Asynchronously opens the Url in the browser (if specified) or the default browser.
    /// </summary>
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
    public async Task<UrlResult> OpenAsync()
    {
        if (BrowserOpenCommand is not null)
        {
            char quoteMark = '"';
            string url = $"{quoteMark}{Url.ToString()}{quoteMark}";
            
            var result = await Cli.Run(BrowserOpenCommand)
                .WithArguments(url)
                .ExecuteAsync();

            return await Task.FromResult(new UrlResult(result.ExitCode, result.StartTime, result.ExitTime,
                Url.ToString()));
        }
        else
        {
            return await OpenInDefaultBrowserAsync();
        }
    }
}