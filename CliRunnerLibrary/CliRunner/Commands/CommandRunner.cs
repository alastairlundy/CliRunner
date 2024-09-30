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
using System.Runtime.Versioning;
using System.Text;

using CliRunner.Commands.Abstractions;
using CliRunner.Processes.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace CliRunner.Commands
{

    /// <summary>
    /// 
    /// </summary>
    public class CommandRunner : ICommandRunner
    {
        protected IProcessRunner processRunner;

        public CommandRunner()
        {
            processRunner = new ProcessRunner();
        }

        public CommandRunner(IProcessRunner processRunner)
        {
            this.processRunner = processRunner;
        }

        /// <summary>
        /// Run a command on macOS located in the /usr/bin/ folder.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="runAsAdministrator"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
    #endif
     public ProcessResult RunCommandOnMac(string command, bool runAsAdministrator = false)
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException();
            }
            
            string location = "/usr/bin/";
                    
            string[] array = command.Split(' ');

            if (array.Length > 1)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string argument in array)
                {
                    stringBuilder.Append($"{argument} ");
                }

                string args = stringBuilder.ToString().Replace(array[0], string.Empty);
                    
                return processRunner.RunProcessOnMac(location, array[0], args);
            }
            else
            {
                if (runAsAdministrator)
                {
                    command = command.Insert(0, "sudo ");
                }
                    
                return processRunner.RunProcessOnMac(location, command);
            }
        }

        /// <summary>
        /// Run a command or program as if inside a terminal on FreeBSD.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="runAsAdministrator"></param>
        /// <returns></returns>
    #if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
    #endif
        public ProcessResult RunCommandOnFreeBsd(string command, bool runAsAdministrator = false)
        {
            return RunCommandOnLinux(command, runAsAdministrator);
        }
        

        /// <summary>
        /// Run a command or program as if inside a terminal on Linux.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="runAsAdministrator"></param>
        /// <returns></returns>
    #if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
    #endif
        public ProcessResult RunCommandOnFreeBsd(Command command, bool runAsAdministrator = false)
        {
            if (OperatingSystem.IsLinux() == false && OperatingSystem.IsFreeBSD() == false)
            {
                throw new PlatformNotSupportedException();
            }
            
            string location = "/usr/bin/";

            string[] args = command.Split(' ');
            command = args[0];

            StringBuilder stringBuilder = new StringBuilder();
                
            if (args.Length > 0)
            {
                for (int index = 1; index < args.Length; index++)
                {
                    stringBuilder.Append(args[index].Replace(command, string.Empty));
                }
            }

            string processArguments = stringBuilder.ToString();

            if (!Directory.Exists(location))
            {
                throw new DirectoryNotFoundException("Could not find directory " + location);
            }

            if (runAsAdministrator)
            {
                command = command.Insert(0, "sudo ");
            }
                        
            return processRunner.RunProcessOnLinux(location, command, processArguments);
        }
    }        
}