/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
   */

using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using CliRunner.Processes.Abstractions;
using CliRunner.Specializations.Abstractions;


#if NETSTANDARD2_1
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace CliRunner.Specializations
{
    public class ClassicPowershellRunner : IClassicPowershellRunner
    {
        protected IProcessRunner processRunner;

        public ClassicPowershellRunner()
        {
            processRunner = new ProcessRunner();
        }

        public ClassicPowershellRunner(IProcessRunner processRunner)
        {
            this.processRunner = processRunner;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public IProcessResult Execute(string command, bool runAsAdministrator)
        {
            return processRunner.RunProcessOnWindows(GetInstallLocation(),
                "powershell", command, null, 
                runAsAdministrator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public string GetInstallLocation()
        {
            if (OperatingSystem.IsWindows())
            {
                return $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}" +
                       $"System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0";
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsInstalled()
        {
            if (OperatingSystem.IsWindows())
            {
                string installLocation = GetInstallLocation();

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

        public Version GetInstalledVersion()
        {
            throw new NotImplementedException();
        }
    }
}