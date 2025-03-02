/*
    CliInvoke
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Command.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Command.cs

     Constructor signature and field declarations from CliWrap licensed under the MIT License except where considered Copyright Fair Use by law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AlastairLundy.CliInvoke.Internal.Localizations;

using AlastairLundy.Extensions.Processes;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable ArrangeObjectCreationWhenTypeEvident

namespace AlastairLundy.CliInvoke
{
    /// <summary>
    /// A class to represent Commands that can be run.
    /// </summary>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public class CliCommandConfiguration : IEquatable<CliCommandConfiguration>
    {
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
        public UserCredential Credential { get; protected set; }

        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public ProcessResultValidation ResultValidation { get; protected set; }

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
        /// The Standard Error target.
        /// </summary>
        public StreamReader StandardError { get; protected set; }

        /// <summary>
        /// The Process Resource Policy to be used for executing the Command.
        /// </summary>
        public ProcessResourcePolicy ResourcePolicy { get; protected set; }

        /// <summary>
        /// Whether to use Shell Execution or not.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public bool UseShellExecution { get; protected set; }
        
        /// <summary>
        /// The encoding to use for the Standard Input.
        /// </summary>
        public Encoding StandardInputEncoding { get; protected set; }
        
        /// <summary>
        /// The encoding to use for the Standard Output.
        /// </summary>
        public Encoding StandardOutputEncoding { get; protected set; }
        
        /// <summary>
        /// The encoding to use for the Standard Error.
        /// </summary>
        public Encoding StandardErrorEncoding { get; protected set; }

        /// <summary>
        /// Configures the Command configuration to be wrapped and executed.
        /// </summary>
        /// <param name="targetFilePath">The target file path of the command to be executed.</param>
        /// <param name="arguments">The arguments to pass to the Command upon execution.</param>
        /// <param name="workingDirectoryPath">The working directory to be used.</param>
        /// <param name="requiresAdministrator">Whether to run the Command with Administrator Privileges.</param>
        /// <param name="environmentVariables">The environment variables to be set (if specified).</param>
        /// <param name="credential">The credentials to be used (if specified).</param>
        /// <param name="commandResultValidation">Enables or disables Result Validation and exception throwing if the task exits unsuccessfully.</param>
        /// <param name="standardInput">The standard input source to be used (if specified).</param>
        /// <param name="standardOutput">The standard output destination to be used (if specified).</param>
        /// <param name="standardError">The standard error destination to be used (if specified).</param>
        /// <param name="processResourcePolicy">The process resource policy to be used (if specified).</param>
        /// <param name="windowCreation">Whether to enable or disable Window Creation by the Command's Process.</param>
        /// <param name="useShellExecution">Whether to enable or disable executing the Command through Shell Execution.</param>
        /// <param name="standardInputEncoding">The Standard Input Encoding to be used (if specified).</param>
        /// <param name="standardOutputEncoding">The Standard Output Encoding to be used (if specified).</param>
        /// <param name="standardErrorEncoding">The Standard Error Encoding to be used (if specified).</param>
        public CliCommandConfiguration(string targetFilePath,
            string arguments = null, string workingDirectoryPath = null,
            bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
            UserCredential credential = null,
            ProcessResultValidation commandResultValidation = ProcessResultValidation.ExitCodeZero,
            StreamWriter standardInput = null,
            StreamReader standardOutput = null,
            StreamReader standardError = null,
            Encoding standardInputEncoding = null,
            Encoding standardOutputEncoding = null,
            Encoding standardErrorEncoding = null,
            ProcessResourcePolicy processResourcePolicy = null,
            bool windowCreation = false,
            bool useShellExecution = false
        )
        {
            TargetFilePath = targetFilePath;
            RequiresAdministrator = requiresAdministrator;
            Arguments = arguments ?? string.Empty;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credential = credential ?? UserCredential.Null;
            
            ResourcePolicy = processResourcePolicy ?? ProcessResourcePolicy.Default;

            ResultValidation = commandResultValidation;

            StandardInput = standardInput ?? StreamWriter.Null;
            StandardOutput = standardOutput ?? StreamReader.Null;
            StandardError = standardError ?? StreamReader.Null;
            
            UseShellExecution = useShellExecution;
            WindowCreation = windowCreation;
            
            StandardInputEncoding = standardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = standardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = standardErrorEncoding ?? Encoding.Default;
        }

        /// <summary>
        /// Creates a new Command with the specified Command Configuration.
        /// </summary>
        /// <param name="commandConfiguration">The command configuration to be used to for the Command.</param>
        public CliCommandConfiguration(CliCommandConfiguration commandConfiguration)
        {
            TargetFilePath = commandConfiguration.TargetFilePath;
            Arguments = commandConfiguration.Arguments; 
            WorkingDirectoryPath = commandConfiguration.WorkingDirectoryPath ?? Directory.GetCurrentDirectory();
            RequiresAdministrator = commandConfiguration.RequiresAdministrator;
            EnvironmentVariables = commandConfiguration.EnvironmentVariables  ?? new Dictionary<string, string>();
            Credential = commandConfiguration.Credential ?? UserCredential.Null;
            ResultValidation = commandConfiguration.ResultValidation;
            StandardInput = commandConfiguration.StandardInput ?? StreamWriter.Null;
            StandardOutput = commandConfiguration.StandardOutput ?? StreamReader.Null;
            StandardError = commandConfiguration.StandardError ?? StreamReader.Null;
            
            StandardInputEncoding = commandConfiguration.StandardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = commandConfiguration.StandardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = commandConfiguration.StandardErrorEncoding ?? Encoding.Default;
            
            ResourcePolicy = commandConfiguration.ResourcePolicy;
            WindowCreation = commandConfiguration.WindowCreation;
            UseShellExecution = commandConfiguration.UseShellExecution;
        }

        /// <summary>
        /// Returns a string representation of the configuration of the Command.
        /// </summary>
        /// <returns>A string representation of the configuration of the Command</returns>
        public override string ToString()
        {
            string commandString = $"{TargetFilePath} {Arguments}";
            string workingDirectory = string.IsNullOrEmpty(WorkingDirectoryPath) ? "" : $" ({Resources.Command_ToString_WorkingDirectory}: {WorkingDirectoryPath})";
            string adminPrivileges = RequiresAdministrator ? $"{Environment.NewLine} {Resources.Command_ToString_RequiresAdmin}" : "";
            string shellExecution = UseShellExecution ? $"{Environment.NewLine} {Resources.Command_ToString_ShellExecution}" : "";

            return $"{commandString}{workingDirectory}{adminPrivileges}{shellExecution}";
        }

        /// <summary>
        /// Returns whether this Command object is equal to another Command object.
        /// </summary>
        /// <param name="other">The other Command object to be compared.</param>
        /// <returns>true if both Command objects are the same; false otherwise.</returns>
        public bool Equals(CliCommandConfiguration other)
        {
            if (other is null)
            {
                return false;
            }
            
            return RequiresAdministrator == other.RequiresAdministrator
                   && TargetFilePath == other.TargetFilePath
                   && WorkingDirectoryPath == other.WorkingDirectoryPath
                   && Arguments == other.Arguments
                   && WindowCreation == other.WindowCreation
                   && Equals(EnvironmentVariables, other.EnvironmentVariables)
                   && Equals(Credential, other.Credential)
                   && ResultValidation == other.ResultValidation
                   && Equals(StandardInput, other.StandardInput)
                   && Equals(StandardOutput, other.StandardOutput)
                   && Equals(StandardError, other.StandardError)
                   && ResourcePolicy == other.ResourcePolicy
                   && UseShellExecution == other.UseShellExecution;
        }

        /// <summary>
        /// Returns whether this Command object is equal to another object.
        /// </summary>
        /// <param name="obj">The other object to be compared.</param>
        /// <returns>true if both objects are Command objects and the same; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is CliCommandConfiguration command)
            {
                return Equals(command);
            }

            return false;
        }

        /// <summary>
        /// Returns the hashcode of this Command object.
        /// </summary>
        /// <returns>the hashcode of this Command object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RequiresAdministrator.GetHashCode();
                hashCode = (hashCode * 397) ^ (TargetFilePath != null ? TargetFilePath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (WorkingDirectoryPath != null ? WorkingDirectoryPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Arguments != null ? Arguments.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ WindowCreation.GetHashCode();
                hashCode = (hashCode * 397) ^ (EnvironmentVariables != null ? EnvironmentVariables.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Credential != null ? Credential.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)ResultValidation;
                hashCode = (hashCode * 397) ^ (StandardInput != null ? StandardInput.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StandardOutput != null ? StandardOutput.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StandardError != null ? StandardError.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ResourcePolicy.GetHashCode();
                hashCode = (hashCode * 397) ^ UseShellExecution.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Determines if a Command is equal to another command.
        /// </summary>
        /// <param name="left">A command to be compared.</param>
        /// <param name="right">The other command to be compared.</param>
        /// <returns>True if both Commands are equal to each other; false otherwise.</returns>
        public static bool Equals(CliCommandConfiguration left, CliCommandConfiguration right)
        {
            return left.Equals(right);
        }
        
        /// <summary>
        /// Determines if a Command is equal to another command.
        /// </summary>
        /// <param name="left">A command to be compared.</param>
        /// <param name="right">The other command to be compared.</param>
        /// <returns>True if both Commands are equal to each other; false otherwise.</returns>
        public static bool operator ==(CliCommandConfiguration left, CliCommandConfiguration right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if a Command is not equal to another command.
        /// </summary>
        /// <param name="left">A command to be compared.</param>
        /// <param name="right">The other command to be compared.</param>
        /// <returns>True if both Commands are not equal to each other; false otherwise.</returns>
        public static bool operator !=(CliCommandConfiguration left, CliCommandConfiguration right)
        {
            return Equals(left, right) == false;
        }
    }
}