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
using CliRunner.Extensibility;
using CliRunner.Specializations.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif

namespace CliRunner.Specializations.Configurations
{
    /// <summary>
    /// A class to make running commands through Windows CMD easier.
    /// </summary>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("linux")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("tvos")]
    [UnsupportedOSPlatform("watchos")]
#endif
    public class CmdCommandConfiguration : SpecializedCommandConfiguration
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
        public CmdCommandConfiguration(string arguments = null,
            string workingDirectoryPath = null, bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null, UserCredential credentials = null,
            CommandResultValidation resultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null, StreamReader standardOutput = null, StreamReader standardError = null,
            Encoding standardInputEncoding = default, Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default, IntPtr processorAffinity = default(IntPtr),
            bool useShellExecution = false, bool windowCreation = false) : 
            base("", arguments,
            workingDirectoryPath,
            requiresAdministrator, environmentVariables, credentials, resultValidation, standardInput, standardOutput,
            standardError, standardInputEncoding, standardOutputEncoding, standardErrorEncoding, processorAffinity,
            useShellExecution, windowCreation)
        {
            
        }

        
        /// <summary>
        /// The target file path of Cmd.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Windows based operating system.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        public new string TargetFilePath
        {
            get
            {
                if (OperatingSystem.IsWindows() == false)
                {
                    throw new PlatformNotSupportedException(Resources.Exceptions_Cmd_OnlySupportedOnWindows);
                }

                return $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}cmd.exe"; ;
            }
        }
    }
}