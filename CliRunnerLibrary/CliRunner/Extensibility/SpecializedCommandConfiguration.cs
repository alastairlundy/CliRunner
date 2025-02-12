/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using CliRunner.Abstractions;

namespace CliRunner.Extensibility;

/// <summary>
/// An abstract class that implements ICommandConfiguration and adds a default constructor.
/// </summary>
/// /// <remarks>Do not use this class directly unless you are creating a specialized Command,
/// such as one that will be run through an intermediary process like Powershell or Cmd.</remarks>
public abstract class SpecializedCommandConfiguration : ICommandConfiguration
{
    /// <summary>
    /// Initializes a new instance of the Specialized Command Configuration class.
    /// </summary>
    /// <param name="targetFilePath">The path to the command executable file.</param>
    /// <param name="arguments">The arguments to be passed to the command.</param>
    /// <param name="workingDirectoryPath">The working directory for the command.</param>
    /// <param name="requiresAdministrator">Indicates whether the command requires administrator privileges.</param>
    /// <param name="environmentVariables">A dictionary of environment variables to be set for the command.</param>
    /// <param name="credentials">The user credentials to be used when running the command.</param>
    /// <param name="commandResultValidation">The validation criteria for the command result.</param>
    /// <param name="standardInput">The stream for the standard input.</param>
    /// <param name="standardOutput">The stream for the standard output.</param>
    /// <param name="standardError">The stream for the standard error.</param>
    /// <param name="standardInputEncoding">The encoding for the standard input stream.</param>
    /// <param name="standardOutputEncoding">The encoding for the standard output stream.</param>
    /// <param name="standardErrorEncoding">The encoding for the standard error stream.</param>
    /// <param name="processResourcePolicy">The Process Resource Policy to be used for the command.</param>
    /// <param name="useShellExecution">Indicates whether to use the shell to execute the command.</param>
    /// <param name="windowCreation">Indicates whether to create a new window for the command.</param>
    /// <remarks>Do not use directly unless you are creating a specialized Command, such as one that will be run through an intermediary like Powershell or Cmd.</remarks>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public SpecializedCommandConfiguration(string targetFilePath, string arguments = null,
        string workingDirectoryPath = null, bool requiresAdministrator = false,
        IReadOnlyDictionary<string, string> environmentVariables = null, UserCredential credentials = null,
        ProcessResultValidation commandResultValidation = ProcessResultValidation.ExitCodeZero,
        StreamWriter standardInput = null, StreamReader standardOutput = null, StreamReader standardError = null,
        Encoding standardInputEncoding = null, Encoding standardOutputEncoding = null,
        Encoding standardErrorEncoding = null, ProcessResourcePolicy processResourcePolicy = null,
        bool useShellExecution = false, bool windowCreation = false)
    {
        TargetFilePath = targetFilePath;
        Arguments = arguments;
        WorkingDirectoryPath = workingDirectoryPath;
        RequiresAdministrator = requiresAdministrator;
        EnvironmentVariables = environmentVariables;
        Credential = credentials;
        ResultValidation = commandResultValidation;
        UseShellExecution = useShellExecution;
        WindowCreation = windowCreation;
        
        StandardInput = standardInput ?? StreamWriter.Null;
        StandardOutput = standardOutput ?? StreamReader.Null;
        StandardError = standardError ?? StreamReader.Null;
        
        StandardInputEncoding = standardInputEncoding ?? Encoding.Default;
        StandardOutputEncoding = standardOutputEncoding ?? Encoding.Default;
        StandardErrorEncoding = standardErrorEncoding ?? Encoding.Default;
        
        ResourcePolicy = processResourcePolicy;
    }

        /// <summary>
        /// Whether administrator privileges are required when executing the Command.
        /// </summary>
        public bool RequiresAdministrator { get; }

        /// <summary>
        /// The file path of the executable to be run and wrapped.
        /// </summary>
        public string TargetFilePath { get; }

        /// <summary>
        /// The working directory path to be used when executing the Command.
        /// </summary>
        public string WorkingDirectoryPath { get; }

        /// <summary>
        /// The arguments to be provided to the executable to be run.
        /// </summary>
        public string Arguments { get; }

        /// <summary>
        /// Whether to enable window creation or not when the Command's Process is run.
        /// </summary>
        public bool WindowCreation { get; }

        /// <summary>
        /// The environment variables to be set.
        /// </summary>
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        /// <summary>
        /// The credentials to be used when executing the executable.
        /// </summary>
        public UserCredential Credential { get; }

        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public ProcessResultValidation ResultValidation { get; }

        /// <summary>
        /// The Standard Input source.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public StreamWriter StandardInput { get; }

        /// <summary>
        /// The Standard Output target.
        /// </summary>
        public StreamReader StandardOutput { get; }

        /// <summary>
        /// The Standard Error target.
        /// </summary>
        public StreamReader StandardError { get; }

        /// <summary>
        /// The processor threads to be used for executing the Command.
        /// </summary>
#if NET6_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
#endif
        public ProcessResourcePolicy ResourcePolicy { get; }

        /// <summary>
        /// Whether to use Shell Execution or not.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public bool UseShellExecution { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Input.
        /// </summary>
        public Encoding StandardInputEncoding { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Output.
        /// </summary>
        public Encoding StandardOutputEncoding { get; }
        
        /// <summary>
        /// The encoding to use for the Standard Error.
        /// </summary>
        public Encoding StandardErrorEncoding { get;  }
}