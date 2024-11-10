/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

using CliRunner.Commands.Abstractions;
using CliRunner.Processes;
using CliRunner.Processes.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
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
        public ProcessResult RunCommandOnWindows(Command command, bool runAsAdministrator = false)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException();
            }

            if (command.SupportsWindows == false)
            {
                throw new PlatformNotSupportedException($"Command {command.Name} declared that it does not support windows.");
            }
            
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
            ProcessStartInfo? startInfo;    
#elif NETSTANDARD2_0
            ProcessStartInfo startInfo;
#endif

            if (command.StartInfo != null)
            {
                startInfo = command.StartInfo;
            }
            else
            {
                startInfo = null;
            }
            
            if (runAsAdministrator)
            {
                if (startInfo != null)
                {
                    startInfo.Verb = "runas";
                }
            }

            return _processRunner.RunProcessOnWindows(command.FilePath,
                command.Name, command.Arguments, startInfo, runAsAdministrator);
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
            List<string> args = command.Split(' ').ToList();
            string commandName = args[0];
            
            args.RemoveAt(0);
            
            string location = "/usr/bin/";

            Command actualCommand = new Command(commandName, location,
                args, false, false, true); 
         
            return RunCommandOnMac(actualCommand, runAsAdministrator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="runAsAdministrator"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public ProcessResult RunCommandOnMac(Command command, bool runAsAdministrator = false)
        {
            if (OperatingSystem.IsMacOS() == false || command.SupportsMac == false)
            {
                throw new PlatformNotSupportedException();
            }

            string args = "";
            
            string commandName = command.Name;
            
            if (runAsAdministrator)
            {
                commandName = command.Name.Insert(0, "sudo ");
            }
            
            if (command.Arguments.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string argument in command.Arguments)
                {
                    stringBuilder.Append($"{argument} ");
                }
                
                args = stringBuilder.ToString().Replace(command.Arguments.ElementAt(0), string.Empty);
            }
            
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
         ProcessStartInfo? startInfo;    
#elif NETSTANDARD2_0
            ProcessStartInfo startInfo;
#endif

            if (command.StartInfo != null)
            {
                startInfo = command.StartInfo;
            }
            else
            {
                startInfo = null;
            }

            return processRunner.RunProcessOnMac(command.FilePath, commandName, args, startInfo);
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

        public ProcessResult RunCommandOnFreeBsd(Command command, bool runAsAdministrator = false)
        {
            return RunCommandOnLinux(command, runAsAdministrator);
        }

        public ProcessResult RunCommandOnLinux(Command command, bool runAsAdministrator = false)
        {
            if (OperatingSystem.IsLinux() == false && OperatingSystem.IsFreeBSD() == false && command.SupportsLinux == false)
            {
                throw new PlatformNotSupportedException();
            }

            string commandName = command.Name;
            string args = "";

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
         ProcessStartInfo? startInfo;    
#elif NETSTANDARD2_0
            ProcessStartInfo startInfo;
#endif
            
            if (runAsAdministrator)
            {
                commandName = commandName.Insert(0, "sudo ");
            }
            
            if (command.Arguments.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (string argument in command.Arguments)
                {
                    stringBuilder.Append($"{argument} ");
                }
                
                args = stringBuilder.ToString().Replace(command.Arguments.ElementAt(0), string.Empty);
            }
            
            if (Directory.Exists(command.FilePath) == false)
            {
                throw new DirectoryNotFoundException("Could not find directory " + command.FilePath);
            }

            if (command.StartInfo != null)
            {
                startInfo = command.StartInfo;
            }
            else
            {
                startInfo = null;
            }
                        
            return processRunner.RunProcessOnLinux(command.FilePath, commandName, args, startInfo);
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
        public ProcessResult RunCommandOnLinux(string command, bool runAsAdministrator = false)
        {
            string location = "/usr/bin/";
            
            List<string> args = command.Split(' ').ToList();
            
            string commandName = args[0];

            args.RemoveAt(0);
            
            Command actualCommand = new Command(commandName, location, args, false, true, false);
            
            return RunCommandOnLinux(actualCommand, runAsAdministrator);
        }
    }        
}