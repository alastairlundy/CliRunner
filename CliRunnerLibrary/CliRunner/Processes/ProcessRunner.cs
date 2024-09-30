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
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner.Processes;
using CliRunner.Processes.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

// ReSharper disable InvalidXmlDocComment

namespace CliRunner
{
    /// <summary>
    ///  A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessRunner : IProcessRunner
    {
        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments"></param>
        /// <param name="processStartInfo"></param>
        /// <param name="runAsAdministrator"></param>
        /// <param name="insertExeInExecutableNameIfMissing"></param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        /// <exception cref="Exception"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public ProcessResult RunProcessOnWindows(string executableLocation, string executableName,
            string arguments = "", ProcessStartInfo? processStartInfo = null,
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true)
        {
            Process process;
            
            if (processStartInfo != null)
            {
                processStartInfo.WorkingDirectory = executableLocation;
                processStartInfo.FileName = executableName;
                processStartInfo.Arguments = arguments;
                processStartInfo.RedirectStandardOutput = true;
                process = new Process { StartInfo = processStartInfo};
            }
            else
            {
                process = new Process();
                    
                process.StartInfo.FileName = executableName;

                if (!executableName.EndsWith(".exe") && insertExeInExecutableNameIfMissing)
                {
                    process.StartInfo.FileName = $"{process.StartInfo.FileName}.exe";
                }

                process.StartInfo.WorkingDirectory = executableLocation;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.Arguments = arguments;
            }

            if (runAsAdministrator)
            {
                process.StartInfo.Verb = "runas";
            }

            if (!Directory.Exists(executableLocation))
            {
                throw new DirectoryNotFoundException();
            }

            Task task = new Task(() => process.Start());
            task.Start();

            task.Wait();
            
            string end = process.StandardOutput.ReadToEnd();

            if (end == null)
            {
                throw new NullReferenceException();
            }

            if (end.ToLower()
                .Contains("is not recognized as the name of a cmdlet, function, script file, or operable program"))
            {
                throw new Exception(end);
            }
            else if (end.ToLower()
                     .Contains(
                         "is not recognized as an internal or external command, operable program or batch file."))
            {
                throw new Exception(end);
            }

            ProcessResult output = new ProcessResult(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StartTime, process.ExitTime);


            return output;
        }

        /// <summary>
        ///  Run a Process on macOS
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        public ProcessResult RunProcessOnMac(string executableLocation, string executableName, string arguments = "", ProcessStartInfo? processStartInfo = null)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = executableLocation,
                FileName = executableName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = arguments
            };

            Process process;
            
            if (processStartInfo == null)
            {
                process = new Process { StartInfo = procStartInfo };
            }
            else
            {
                procStartInfo = processStartInfo;
                procStartInfo.WorkingDirectory = executableLocation;
                procStartInfo.FileName = executableName;
                procStartInfo.Arguments = arguments;
                procStartInfo.RedirectStandardOutput = true;
                process = new Process { StartInfo = processStartInfo};
            }
            
            process.Start();

            process.WaitForExit();
            
            ProcessResult output = new ProcessResult(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StartTime, process.ExitTime);
            
            return output;
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ProcessResult RunProcessOnLinux(string executableLocation, string executableName, string arguments = "", ProcessStartInfo? processStartInfo = null)
        {
            ProcessStartInfo procStartInfo;
                
            if (processStartInfo == null)
            {
                procStartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = executableLocation,
                    FileName = executableName,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    Arguments = arguments
                };
            }
            else
            {
                procStartInfo = processStartInfo;
                procStartInfo.WorkingDirectory = executableLocation;
                procStartInfo.FileName = executableName;
                procStartInfo.Arguments = arguments;
                procStartInfo.RedirectStandardOutput = true;
            }
                
            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
            
            process.WaitForExit();

            ProcessResult output = new ProcessResult(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StartTime, process.ExitTime);

            return output;
        }

        /// <summary>
        /// Run a Process on FreeBSD
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
        public ProcessResult RunProcessOnFreeBsd(string executableLocation, string executableName,
            string arguments = "", ProcessStartInfo? processStartInfo = null)
        {
            return RunProcessOnLinux(executableLocation, executableName, arguments, processStartInfo);
        }
    }
}