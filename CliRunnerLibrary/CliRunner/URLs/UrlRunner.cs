/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CliRunner.Commands;
using CliRunner.Commands.Abstractions;
using CliRunner.Specializations;
using CliRunner.Specializations.Abstractions;

using CliRunner.URLs.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.URLs
{

    public class UrlRunner : IUrlRunner
    {
        protected ICmdRunner cmdRunner;
        protected ICommandRunner commandRunner;

        public UrlRunner()
        {
            cmdRunner = new CmdRunner();
            commandRunner = new CommandRunner();
        }

        public UrlRunner(ICmdRunner cmdRunner, ICommandRunner commandRunner)
        {
            this.cmdRunner = cmdRunner;
            this.commandRunner = commandRunner;
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
        public void OpenUrlInDefaultBrowser(string url)
        {
            url = AddHttpIfMissing(url, false);
            url = ReplaceHttpWithHttps(url);
            
            OpenUrl(url);
        }

        protected void OpenUrl(string url)
        {
            if (OperatingSystem.IsWindows())
            {
               cmdRunner.Execute($"/c start {url.Replace("&", "^&")}", false);
            }
            if (OperatingSystem.IsLinux())
            {
                commandRunner.RunCommandOnLinux($"xdg-open {url}");
            }
            if (OperatingSystem.IsMacOS())
            {
                Task task = new Task(() => Process.Start("open", url));
                task.Start();
            }
            if (OperatingSystem.IsFreeBSD())
            {
                commandRunner.RunCommandOnFreeBsd($"xdg-open {url}");
            }
        }

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