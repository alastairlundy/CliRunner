/*
    CliRunner Specializations
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CliRunner.Abstractions;

using CliRunner.Specializations.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif

namespace CliRunner.Specializations.Configurations
{
    /// <summary>
    /// A class to make running commands through Windows Powershell easier.
    /// </summary>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("android")]
#endif
    public class ClassicPowershellCommandConfiguration : ICommandConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requiresAdministrator"></param>
        /// <param name="workingDirectoryPath"></param>
        /// <param name="arguments"></param>
        /// <param name="environmentVariables"></param>
        /// <param name="credentials"></param>
        /// <param name="resultValidation"></param>
        /// <param name="standardInput"></param>
        /// <param name="standardOutput"></param>
        /// <param name="standardError"></param>
        /// <param name="useShellExecution"></param>
        /// <param name="windowCreation"></param>
        /// <param name="standardInputEncoding"></param>
        /// <param name="standardOutputEncoding"></param>
        /// <param name="standardErrorEncoding"></param>
        /// <param name="processorAffinity"></param>
        public ClassicPowershellCommandConfiguration(string arguments,
            string workingDirectoryPath = null, bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null, UserCredentials credentials = null,
            CommandResultValidation resultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null, StreamReader standardOutput = null, StreamReader standardError = null,
            Encoding standardInputEncoding = default, Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default, IntPtr processorAffinity = default(IntPtr),
            bool useShellExecution = false, bool windowCreation = false)
        {
            RequiresAdministrator = requiresAdministrator;
            WorkingDirectoryPath = workingDirectoryPath;
            Arguments = arguments;
            EnvironmentVariables = environmentVariables;
            Credentials = credentials;
            ResultValidation = resultValidation;
            StandardInput = standardInput;
            StandardOutput = standardOutput;
            StandardError = standardError;
            UseShellExecution = useShellExecution;
            WindowCreation = windowCreation;
            StandardInputEncoding = standardInputEncoding;
            StandardOutputEncoding = standardOutputEncoding;
            StandardErrorEncoding = standardErrorEncoding;
            ProcessorAffinity = processorAffinity;
        }
        
        /// <summary>
        /// The target file path of Windows Powershell.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Windows based operating system.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("android")]
#endif
        // ReSharper disable once MemberCanBePrivate.Global
        public string TargetFilePath
        {
            get
            {
                if (OperatingSystem.IsWindows() == false)
                {
                    throw new PlatformNotSupportedException(Resources.Exceptions_ClassicPowershell_OnlySupportedOnWindows);
                }

                string location = $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}" +
                                  $"System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0";
                       
                return $"{location}{Path.DirectorySeparatorChar}powershell.exe";
            }
        }
        
        public bool RequiresAdministrator { get; }
        public string WorkingDirectoryPath { get; }
        public string Arguments { get; }
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
        public UserCredentials Credentials { get; }
        public CommandResultValidation ResultValidation { get; }
        public StreamWriter StandardInput { get; }
        public StreamReader StandardOutput { get; }
        public StreamReader StandardError { get; }
        public bool UseShellExecution { get; }
        public bool WindowCreation { get; }
        public Encoding StandardInputEncoding { get; }
        public Encoding StandardOutputEncoding { get; }
        public Encoding StandardErrorEncoding { get; }
        public IntPtr ProcessorAffinity { get; }
    }
}