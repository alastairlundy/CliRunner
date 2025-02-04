/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
    
    Based on Tyrrrz's CliWrap Command.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Command.cs

     Constructor signature and Field from CliWrap licensed under the MIT License except where considered Copyright Fair Use By Law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace CliRunner.Abstractions;

/// <summary>
/// An abstract class that implements ICommandConfiguration and adds a default constructor.
/// </summary>
/// <remarks>Do not use this class directly unless you are creating a specialized Command,
/// such as one that will be run through an intermediary process like Powershell or Cmd.</remarks>
public abstract class AbstractCommandConfiguration : ICommandConfiguration
{
    
    /// <summary>
    /// Instantiates an Abstract Command with default values for most parameters.
    /// </summary>
    /// <param name="targetFilePath">The target file path of the command to be executed.</param>
    /// <param name="arguments">The arguments to pass to the Command upon execution.</param>
    /// <param name="workingDirectoryPath">The working directory to be used.</param>
    /// <param name="requiresAdministrator">Whether to run the Command with Administrator Privileges.</param>
    /// <param name="environmentVariables">The environment variables to be set (if specified).</param>
    /// <param name="credentials">The credentials to be used (if specified).</param>
    /// <param name="resultValidation">Enables or disables Result Validation and exception throwing if the task exits unsuccessfully.</param>
    /// <param name="standardInput">The standard input source to be used (if specified).</param>
    /// <param name="standardOutput">The standard output destination to be used (if specified).</param>
    /// <param name="standardError">The standard error destination to be used (if specified).</param>
    /// <param name="processorAffinity">The processor affinity to be used (if specified).</param>
    /// <param name="standardInputEncoding">The encoding to be used for the Standard Input.</param>
    /// <param name="standardOutputEncoding">The encoding to be used for the Standard Output.</param>
    /// <param name="standardErrorEncoding">The encoding to be used for the Standard Error.</param>
    /// <param name="useShellExecution">Whether to enable or disable executing the Command through Shell Execution.</param>
    /// <param name="windowCreation">Whether to enable or disable Window Creation by the Command's Process.</param>
    /// <remarks>Do not use directly unless you are creating a specialized Command, such as one that will be run through an intermediary like Powershell or Cmd.</remarks>
    protected AbstractCommandConfiguration(string targetFilePath, string arguments = null,
            string workingDirectoryPath = null, bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null, UserCredentials credentials = null,
            CommandResultValidation resultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null, StreamReader standardOutput = null, StreamReader standardError = null,
            Encoding standardInputEncoding = null, Encoding standardOutputEncoding = null,
            Encoding standardErrorEncoding = null, IntPtr processorAffinity = default(IntPtr),
            bool useShellExecution = false, bool windowCreation = false)
        {
            TargetFilePath = targetFilePath;
            RequiresAdministrator = requiresAdministrator;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            Arguments = arguments ?? string.Empty;
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credentials = credentials ?? UserCredentials.Default;
            ResultValidation = resultValidation;
            
            StandardInput = standardInput ?? StreamWriter.Null;
            StandardOutput = standardOutput ?? StreamReader.Null;
            StandardError = standardError ?? StreamReader.Null;
            
            UseShellExecution = useShellExecution;
            WindowCreation = windowCreation;
            
            StandardInputEncoding = standardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = standardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = standardErrorEncoding ?? Encoding.Default;
            
            ProcessorAffinity = processorAffinity;
        }
    
        /// <summary>
        /// Whether administrator privileges are required when executing the Command.
        /// </summary>
        public bool RequiresAdministrator { get; protected set; }

        /// <summary>
        /// The file path of the executable to be run and wrapped.
        /// </summary>
        public string TargetFilePath { get; protected set; }

        /// <summary>
        /// The working directory path to be used when executing the Command.
        /// </summary>
        public string WorkingDirectoryPath { get; protected set; }

        /// <summary>
        /// The arguments to be provided to the executable to be run.
        /// </summary>
        public string Arguments { get; protected set; }

        /// <summary>
        /// Whether to enable window creation or not when the Command's Process is run.
        /// </summary>
        public bool WindowCreation { get; protected set; }

        /// <summary>
        /// The environment variables to be set.
        /// </summary>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }

        /// <summary>
        /// The credentials to be used when executing the executable.
        /// </summary>
        public UserCredentials Credentials { get; protected set; }

        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public CommandResultValidation ResultValidation { get; protected set; }

        /// <summary>
        /// The Standard Input source.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public StreamWriter StandardInput { get; protected set; }

        /// <summary>
        /// The Standard Output target.
        /// </summary>
        public StreamReader StandardOutput { get; protected set; }

        /// <summary>
        /// The Standard Error target5.
        /// </summary>
        public StreamReader StandardError { get; protected set; }

        /// <summary>
        /// The processor threads to be used for executing the Command.
        /// </summary>
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
#endif
        public IntPtr ProcessorAffinity { get; protected set; }

        /// <summary>
        /// Whether to use Shell Execution or not.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public bool UseShellExecution { get; protected set; }
        
        /// <summary>
        /// The Encoding to be used for the Standard Input.
        /// </summary>
        public Encoding StandardInputEncoding { get; protected set; }
        
        /// <summary>
        /// The Encoding to be used for the Standard Output.
        /// </summary>
        public Encoding StandardOutputEncoding { get; protected set; }
        
        /// <summary>
        /// The Encoding to be used for the Standard Error.
        /// </summary>
        public Encoding StandardErrorEncoding { get; protected set; }
}