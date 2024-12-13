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

using CliRunner.Urls.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Urls
{

    public class UrlRunner : IUrlRunner
    {
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
        ///
        /// Some code contained courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url"></param>
        public async Task OpenUrlInDefaultBrowserAsync(string url)
        {
            url = AddHttpIfMissing(url, false);
            url = ReplaceHttpWithHttps(url);
            
            if (OperatingSystem.IsWindows())
            {
                string args = $"/c start {url.Replace("&", "^&")}";
                
                await Cli.Wrap($"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}cmd.exe")
                    .WithArguments(args)
                    .WithWorkingDirectory(Environment.SystemDirectory)
                    .ExecuteAsync();
            }
            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                await Cli.Wrap("/usr/bin/xdg-open")
                    .WithArguments(url.Replace("&", "^&"))
                    .WithWorkingDirectory("/usr/bin")
                    .ExecuteAsync();
            }
            if (OperatingSystem.IsMacOS())
            {
                Task task = new Task(() => Process.Start("open", url));
                task.Start();
                
                await task.ConfigureAwait(false);
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