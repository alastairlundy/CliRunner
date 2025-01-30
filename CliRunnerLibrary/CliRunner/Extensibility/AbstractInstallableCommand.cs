/*
    CliRunner 
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable PublicConstructorInAbstractClass
// ReSharper disable PreferConcreteValueOverDefault
// ReSharper disable ArrangeDefaultValueWhenTypeEvident

namespace CliRunner.Extensibility;

/// <summary>
/// Represents an installable Command.
/// </summary>
[Obsolete("This class is deprecated and will be removed in a future version.")]
public abstract class AbstractInstallableCommand : Command
{
    /// <summary>
    /// Initializes a new instance of the AbstractInstallableCommand class.
    /// </summary>
    /// <param name="targetFilePath">The path to the command executable file.</param>
    public AbstractInstallableCommand(string targetFilePath) : base(targetFilePath)
    {
        TargetFilePath = targetFilePath;
    }
        
    /// <summary>
    /// Initializes a new instance of the AbstractInstallableCommand class.
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
    /// <param name="processorAffinity">The processor affinity for the command.</param>
    /// <param name="useShellExecute">Indicates whether to use the shell to execute the command.</param>
    /// <param name="windowCreation">Indicates whether to create a new window for the command.</param>
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
#pragma warning disable CA1416
        ProcessorAffinity = processorAffinity;
#pragma warning restore CA1416
    }

    /// <summary>
    /// Detects whether the Command is installed on the current system.
    /// </summary>
    /// <returns>true if the Command is installed; returns false otherwise.</returns>
    public bool IsInstalled()
    {
        string installLocation = Path.GetFullPath(TargetFilePath);

        try
        {
            return IsCurrentOperatingSystemSupported() &&
                   Directory.Exists(installLocation) &&
                   Directory.GetFiles(installLocation).Contains(TargetFilePath);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the current operating system is supported by the application.
    /// </summary>
    /// <returns>true if the current operating system is supported; otherwise, false.</returns>
    public abstract bool IsCurrentOperatingSystemSupported();

    /// <summary>
    /// Asynchronously retrieves the version of the installed command.
    /// </summary>
    /// <returns>The task result containing the version of the installed software.</returns>
    public abstract Task<Version> GetInstalledVersionAsync();
}