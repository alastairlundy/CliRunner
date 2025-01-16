/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CliRunner.Extensibility
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractInstallableCommand : Command, IInstallableCommand
    {
        public AbstractInstallableCommand(string targetFilePath) : base(targetFilePath)
        {
            TargetFilePath = targetFilePath;
        }
        
        public AbstractInstallableCommand(string targetFilePath, string arguments = null,
            string workingDirectoryPath = null, bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null, UserCredentials credentials = null,
            CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null, StreamReader standardOutput = null, StreamReader standardError = null,
            Encoding standardInputEncoding = default, Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default, IntPtr processorAffinity = default(IntPtr),
            bool useShellExecute = false, bool windowCreation = false)
            : base(targetFilePath, arguments,
            workingDirectoryPath, requiresAdministrator, environmentVariables, credentials, commandResultValidation,
            standardInput, standardOutput, standardError, standardInputEncoding, standardOutputEncoding,
            standardErrorEncoding, processorAffinity, windowCreation)
        {
            TargetFilePath = targetFilePath;
            Arguments = arguments;
            WorkingDirectoryPath = workingDirectoryPath;
            RequiresAdministrator = requiresAdministrator;
            EnvironmentVariables = environmentVariables;
            Credentials = credentials;
            ResultValidation = commandResultValidation;
            UseShellExecution = useShellExecute;
            WindowCreation = windowCreation;
            StandardInput = standardInput;
            StandardOutput = standardOutput;
            StandardError = standardError;
            ProcessorAffinity = processorAffinity;
        }
        
        public abstract Task<string> GetInstallLocationAsync();

        /// <summary>
        /// Detects whether the Command is installed on the current system.
        /// </summary>
        /// <returns>true if the Command is installed; returns false otherwise.</returns>
        public virtual async Task<bool> IsInstalledAsync()
        {
            string installLocation = await GetInstallLocationAsync();

            try
            {
                return Directory.Exists(installLocation) &&
                       Directory.GetFiles(installLocation).Contains(TargetFilePath);
            }
            catch
            {
                return false;
            }
        }

        public abstract Task<Version> GetInstalledVersionAsync();
    }
}