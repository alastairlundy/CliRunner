using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;

using System.Threading.Tasks;
using CliRunner.Commands.Abstractions;
using CliRunner.Piping.Abstractions;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace CliRunner.Commands
{
    public class CommandRunner : ICommandRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
#endif
        public ProcessStartInfo GetStartInfo(Command command)
        {
            ProcessStartInfo output = new ProcessStartInfo()
            {
                FileName = command.TargetFilePath,
                WorkingDirectory = command.WorkingDirectoryPath,
                UseShellExecute = false,
                CreateNoWindow = false,
            };

            if (string.IsNullOrEmpty(command.Arguments) == false)
            {
                output.Arguments = command.Arguments;
            }

            if (command.RunAsAdministrator == true)
            {
                if (OperatingSystem.IsWindows())
                {
                    output.Verb = "runas";
                }
                else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS() || OperatingSystem.IsFreeBSD())
                {
                    output.Verb = "sudo";
                }
            }

            if (command.StandardInputPipe != command.StandardInputPipe.Null)
            {
                output.RedirectStandardInput = true;
            }
            if (command.StandardOutputPipe != command.StandardOutputPipe.Null)
            {
                output.RedirectStandardOutput = true;
            }
            if (command.StandardErrorPipe != command.StandardErrorPipe.Null)
            {
                output.RedirectStandardError = true;
            }

            return output;
        }
        
        public CommandResult Execute(Command command)
        {
            throw new System.NotImplementedException(); 
        }

        public Task<CommandResult> ExecuteAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}