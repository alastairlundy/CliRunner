/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.

    Based on Tyrrrz's CliWrap Command.cs
    https://github.com/Tyrrrz/CliWrap/blob/master/CliWrap/Command.cs

     Constructor signature from CliWrap licensed under the MIT License except where considered Copyright Fair Use By Law.
     See THIRD_PARTY_NOTICES.txt for a full copy of the MIT LICENSE.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using CliRunner.Abstractions;
using CliRunner.Internal.Localizations;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable ArrangeObjectCreationWhenTypeEvident

namespace CliRunner
{
    /// <summary>
    /// A class to represent a Command to be run.
    /// </summary>
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public class Command : AbstractCommandConfiguration, IEquatable<Command>
    {

        /// <summary>
        /// Instantiates a Command to be executed.
        /// </summary>
        /// <param name="targetFilePath">The target file path of the command to be executed.</param>
        /// <param name="arguments">The arguments to pass to the Command upon execution.</param>
        /// <param name="workingDirectoryPath">The working directory to be used.</param>
        /// <param name="requiresAdministrator">Whether to run the Command with Administrator Privileges.</param>
        /// <param name="environmentVariables">The environment variables to be set (if specified).</param>
        /// <param name="credentials">The credentials to be used (if specified).</param>
        /// <param name="commandResultValidation">Enables or disables Result Validation and exception throwing if the task exits unsuccessfully.</param>
        /// <param name="standardInput">The standard input source to be used (if specified).</param>
        /// <param name="standardOutput">The standard output destination to be used (if specified).</param>
        /// <param name="standardError">The standard error destination to be used (if specified).</param>
        /// <param name="processorAffinity">The processor affinity to be used (if specified).</param>
        /// <param name="standardInputEncoding">The encoding to be used for the Standard Input.</param>
        /// <param name="standardOutputEncoding">The encoding to be used for the Standard Output.</param>
        /// <param name="standardErrorEncoding">The encoding to be used for the Standard Error.</param>
        /// <param name="useShellExecution">Whether to enable or disable executing the Command through Shell Execution.</param>
        /// <param name="windowCreation">Whether to enable or disable Window Creation by the Command's Process.</param>
        public Command(string targetFilePath,
            string arguments = null, string workingDirectoryPath = null,
            bool requiresAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
            UserCredentials credentials = null,
            CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null,
            StreamReader standardOutput = null,
            StreamReader standardError = null,
            Encoding standardInputEncoding = null,
            Encoding standardOutputEncoding = null,
            Encoding standardErrorEncoding = null,
#if NET6_0_OR_GREATER && !NET9_0_OR_GREATER
            IntPtr processorAffinity = 0,
#else
             IntPtr processorAffinity = default(IntPtr),
#endif
            bool windowCreation = false,
            bool useShellExecution = false
        ) : base(targetFilePath, arguments, workingDirectoryPath, requiresAdministrator, environmentVariables,
            credentials,
            commandResultValidation, standardInput, standardOutput, standardError, standardInputEncoding,
            standardOutputEncoding, standardErrorEncoding, processorAffinity,  useShellExecution, windowCreation)
        {
            Arguments = arguments ?? string.Empty;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credentials = credentials ?? UserCredentials.Default;
            
            StandardInput = standardInput ?? StreamWriter.Null;
            StandardOutput = standardOutput ?? StreamReader.Null;
            StandardError = standardError ?? StreamReader.Null;

            StandardInputEncoding = standardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = standardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = standardErrorEncoding ?? Encoding.Default;
        }

        /// <summary>
        /// Creates a new Command with the specified Command Configuration.
        /// </summary>
        /// <param name="commandConfiguration">The command configuration to be used to for the Command.</param>
        public Command(ICommandConfiguration commandConfiguration) : base(commandConfiguration.TargetFilePath, 
            commandConfiguration.Arguments, commandConfiguration.WorkingDirectoryPath,
            commandConfiguration.RequiresAdministrator, 
            commandConfiguration.EnvironmentVariables, commandConfiguration.Credentials,
            commandConfiguration.ResultValidation, commandConfiguration.StandardInput,
            commandConfiguration.StandardOutput, commandConfiguration.StandardError,
            commandConfiguration.StandardInputEncoding,
            commandConfiguration.StandardOutputEncoding, commandConfiguration.StandardErrorEncoding,
            commandConfiguration.ProcessorAffinity, commandConfiguration.UseShellExecution,
            commandConfiguration.WindowCreation)
        {
            WorkingDirectoryPath = commandConfiguration.WorkingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = commandConfiguration.EnvironmentVariables  ?? new Dictionary<string, string>();
            Credentials = commandConfiguration.Credentials ?? UserCredentials.Default;

            StandardInput = commandConfiguration.StandardInput ?? StreamWriter.Null;
            StandardOutput = commandConfiguration.StandardOutput ?? StreamReader.Null;
            StandardError = commandConfiguration.StandardError ?? StreamReader.Null;
            
            StandardInputEncoding = commandConfiguration.StandardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = commandConfiguration.StandardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = commandConfiguration.StandardErrorEncoding ?? Encoding.Default;
        }

        /// <summary>
        /// Returns a string representation of the configuration of the Command.
        /// </summary>
        /// <returns>A string representation of the configuration of the Command</returns>
        public override string ToString()
        {
            string commandString = $"{TargetFilePath} {Arguments}";
            string workingDirectory = string.IsNullOrEmpty(WorkingDirectoryPath) ? "" : $" ({Resources.Command_ToString_WorkingDirectory}: {WorkingDirectoryPath})";
            string adminPrivileges = RequiresAdministrator ? $"\n {Resources.Command_ToString_RequiresAdmin}" : "";
            string shellExecution = UseShellExecution ? $"\n {Resources.Command_ToString_ShellExecution}" : "";

            return $"{commandString}{workingDirectory}{adminPrivileges}{shellExecution}";
        }

        /// <summary>
        /// Returns whether this Command object is equal to another Command object.
        /// </summary>
        /// <param name="other">The other Command object to be compared.</param>
        /// <returns>true if both Command objects are the same; false otherwise.</returns>
        public bool Equals(Command other)
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
                   && Equals(Credentials, other.Credentials)
                   && ResultValidation == other.ResultValidation
                   && Equals(StandardInput, other.StandardInput)
                   && Equals(StandardOutput, other.StandardOutput)
                   && Equals(StandardError, other.StandardError)
                   && ProcessorAffinity == other.ProcessorAffinity
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

            if (obj is Command command)
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
                hashCode = (hashCode * 397) ^ (Credentials != null ? Credentials.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)ResultValidation;
                hashCode = (hashCode * 397) ^ (StandardInput != null ? StandardInput.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StandardOutput != null ? StandardOutput.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StandardError != null ? StandardError.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ProcessorAffinity.GetHashCode();
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
        public static bool Equals(Command left, Command right)
        {
            return left.Equals(right);
        }
        
        /// <summary>
        /// Determines if a Command is equal to another command.
        /// </summary>
        /// <param name="left">A command to be compared.</param>
        /// <param name="right">The other command to be compared.</param>
        /// <returns>True if both Commands are equal to each other; false otherwise.</returns>
        public static bool operator ==(Command left, Command right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines if a Command is not equal to another command.
        /// </summary>
        /// <param name="left">A command to be compared.</param>
        /// <param name="right">The other command to be compared.</param>
        /// <returns>True if both Commands are not equal to each other; false otherwise.</returns>
        public static bool operator !=(Command left, Command right)
        {
            return Equals(left, right) == false;
        }
    }
}