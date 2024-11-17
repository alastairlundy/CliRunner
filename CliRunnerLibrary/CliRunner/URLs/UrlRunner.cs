﻿/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner.Commands;
using CliRunner.Commands.Abstractions;

using CliRunner.Urls.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Urls
{

    public class UrlRunner : IUrlRunner
    {
        protected ICommandRunner _commandRunner;

        public UrlRunner()
        {
            _commandRunner = new CommandRunner();
        }

        public UrlRunner(ICommandRunner commandRunner)
        {
            this._commandRunner = commandRunner;
        }
        
        /// <summary>
        /// Convert HTTP to HTTPS
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ReplaceHttpWithHttps(string url)
        {
            if (url.StartsWith("http://"))
            {
                url = url.Replace("http://", "https://");
            }
            else if (url.StartsWith("www."))
            {
                url = url.Replace("www.", "https://www.");
            }

            return url;
        }

        /// <summary>
        /// Open a URL in the default browser.
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url">The URL to be opened.</param>
        #if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("browser")]
        #endif
        public void OpenUrlInDefaultBrowser(string url)
        {
            url = AddHttpIfMissing(url, false);
            url = ReplaceHttpWithHttps(url);
            
            if (OperatingSystem.IsWindows())
            {
                string args = $"/c start {url.Replace("&", "^&")}";
                
                Command cmdCommand = new Command(targetFilePath:"cmd.exe", arguments: args,
                    workingDirectoryPath:Environment.SystemDirectory);
                
                _commandRunner.Execute(cmdCommand);
            }
            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                Command command = new Command("/usr/bin/xdg-open", arguments: url.Replace("&", "^&"), workingDirectoryPath: "/usr/bin");
                _commandRunner.Execute(command);
            }
            if (OperatingSystem.IsMacOS())
            {
                Task task = new Task(() => Process.Start("open", url));
                task.Start();
            }
        }

        /// <summary>
        /// Adds HTTP(S) to a URL if it's missing.
        /// </summary>
        /// <param name="url">The URL to be checked.</param>
        /// <param name="allowNonSecureHttp">Whether to use HTTP instead of HTTPS.</param>
        /// <returns>the modified URL string.</returns>
        public string AddHttpIfMissing(string url, bool allowNonSecureHttp)
        {
            if ((!url.StartsWith("https://") || !url.StartsWith("www.")) && (!url.StartsWith("file://")))
            {
                if (allowNonSecureHttp)
                {
                    url = "http://" + url;
                }
                else
                {
                    url = "https://" + url;
                }
            }

            return url;
        }
    }        
}