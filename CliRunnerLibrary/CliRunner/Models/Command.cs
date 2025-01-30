/*
    CliRunner
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
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Internal.Localizations;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable ArrangeObjectCreationWhenTypeEvident

namespace CliRunner
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public class Command : ICommandConfiguration, IEquatable<Command>
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
        public UserCredentials Credentials { get; protected set; }

        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public CommandResultValidation ResultValidation { get; protected set; }

        /// <summary>
        /// The piped Standard Input.
        /// </summary>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror" />
        public StreamWriter StandardInput { get; protected set; }

        /// <summary>
        /// The piped Standard Output.
        /// </summary>
        public StreamReader StandardOutput { get; protected set; }

        /// <summary>
        /// The piped Standard Error.
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
        /// 
        /// </summary>
        public Encoding StandardInputEncoding { get; protected set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Encoding StandardOutputEncoding { get; protected set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Encoding StandardErrorEncoding { get; protected set; }

        /// <summary>
        /// Configures the Command to be wrapped and executed.
        /// </summary>
        /// <param name="targetFilePath">The target file path of the command to be executed.</param>
        /// <param name="arguments">The arguments to pass to the Command upon execution.</param>
        /// <param name="workingDirectoryPath">The working directory to be used.</param>
        /// <param name="runAsAdministrator">Whether to run the Command with Administrator Privileges.</param>
        /// <param name="environmentVariables">The environment variables to be set (if specified).</param>
        /// <param name="credentials">The credentials to be used (if specified).</param>
        /// <param name="commandResultValidation">Enables or disables Result Validation and exception throwing if the task exits unsuccessfully.</param>
        /// <param name="standardInput">The standard input source to be used (if specified).</param>
        /// <param name="standardOutput">The standard output destination to be used (if specified).</param>
        /// <param name="standardError">The standard error destination to be used (if specified).</param>
        /// <param name="standardErrorEncoding"></param>
        /// <param name="processorAffinity">The processor affinity to be used (if specified).</param>
        /// <param name="windowCreation">Whether to enable or disable Window Creation by the Command's Process.</param>
        /// <param name="useShellExecute">Whether to enable or disable executing the Command through Shell Execution.</param>
        /// <param name="standardInputEncoding"></param>
        /// <param name="standardOutputEncoding"></param>
        public Command(string targetFilePath,
            string arguments = null, string workingDirectoryPath = null,
            bool runAsAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
            UserCredentials credentials = null,
            CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
            StreamWriter standardInput = null,
            StreamReader standardOutput = null,
            StreamReader standardError = null,
            Encoding standardInputEncoding = default,
            Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default,
#if NET6_0_OR_GREATER && !NET9_0_OR_GREATER
            IntPtr processorAffinity = 0,
#else
             IntPtr processorAffinity = default(IntPtr),
#endif
            bool windowCreation = false,
            bool useShellExecute = false
        )
        {
            TargetFilePath = targetFilePath;
            RequiresAdministrator = runAsAdministrator;
            Arguments = arguments ?? string.Empty;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credentials = credentials ?? UserCredentials.Default;

            ResultValidation = commandResultValidation;

            StandardInput = standardInput ?? StreamWriter.Null;
            StandardOutput = standardOutput ?? StreamReader.Null;
            StandardError = standardError ?? StreamReader.Null;

            ProcessorAffinity = processorAffinity;
            UseShellExecution = useShellExecute;
            WindowCreation = windowCreation;
            
            StandardInputEncoding = standardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = standardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = standardErrorEncoding ?? Encoding.Default;
        }

        /// <summary>
        /// Creates a new Command with the specified Command Configuration.
        /// </summary>
        /// <param name="commandConfiguration"></param>
        public Command(ICommandConfiguration commandConfiguration)
        {
            TargetFilePath = commandConfiguration.TargetFilePath;
            Arguments = commandConfiguration.Arguments; 
            WorkingDirectoryPath = commandConfiguration.WorkingDirectoryPath;
            RequiresAdministrator = commandConfiguration.RequiresAdministrator;
            EnvironmentVariables = commandConfiguration.EnvironmentVariables;
            Credentials = commandConfiguration.Credentials;
            ResultValidation = commandConfiguration.ResultValidation;
            StandardInput = commandConfiguration.StandardInput;
            StandardOutput = commandConfiguration.StandardOutput;
            StandardError = commandConfiguration.StandardError;
            
            StandardInputEncoding = commandConfiguration.StandardInputEncoding ?? Encoding.Default;
            StandardOutputEncoding = commandConfiguration.StandardOutputEncoding ?? Encoding.Default;
            StandardErrorEncoding = commandConfiguration.StandardErrorEncoding ?? Encoding.Default;
            
            ProcessorAffinity = commandConfiguration.ProcessorAffinity;
            WindowCreation = commandConfiguration.WindowCreation;
            UseShellExecution = commandConfiguration.UseShellExecution;
        }
                
        /// <summary>
        /// Creates a Command object with the specified target file path. 
        /// </summary>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as <code>.WithArguments("[your args]")</code> and <code>.WithWorkingDirectory("[your directory]")</code>.</para>
        /// <para>Don't forget to call <code>.ExecuteAsync();</code> or <code>.ExecuteBufferedAsync();</code> when you're ready to execute the Command!</para>
        /// </remarks>
        /// <param name="targetFilePath">The target file path of the executable to wrap.</param>
        /// <returns>A new Command object with the configured Target File Path.</returns>
        [Obsolete("Command's static CreateInstance methods are deprecated and will be removed in a future version. Use Command Builder instead.")]
        public static Command CreateInstance(string targetFilePath)
        {
            return new Command(targetFilePath);
        }

        /// <summary>
        /// Used to wrap an existing Command object when a modified version is desired.
        /// </summary>
        /// <param name="command">The command to wrap</param>
        /// <returns>A new Command object with the configured Command information passed to it.</returns>
        /// <remarks>
        /// <para>Chain appropriate Command methods as needed such as <code>WithArguments("[your args]");</code> and <code>WithWorkingDirectory("[your directory]");</code>.</para>
        /// <para>Don't forget to call <code>.ExecuteAsync();</code> or <code>.ExecuteBufferedAsync();</code> when you're ready to execute the Command!</para>
        /// </remarks>
        [Obsolete("Command's static CreateInstance methods are deprecated and will be removed in a future version.")]
        public static Command CreateInstance(Command command)
        {
           return new Command(command.TargetFilePath, command.Arguments,
                command.WorkingDirectoryPath, command.RequiresAdministrator,
                command.EnvironmentVariables, command.Credentials, command.ResultValidation,
                command.StandardInput, command.StandardOutput, command.StandardError,
                Encoding.Default, Encoding.Default, Encoding.Default, command.ProcessorAffinity,
                command.WindowCreation, command.UseShellExecution);
        }
        
        /// <summary>
        /// Sets the arguments to pass to the executable.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the executable.</param>
        /// <returns>The new Command object with the specified arguments.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithArguments(IEnumerable<string> arguments) =>
            new Command(TargetFilePath,
                string.Join(" ", arguments),
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the arguments to pass to the executable.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the executable.</param>
        /// <param name="escapeArguments">Whether to escape the arguments if escape characters are detected.</param>
        /// <returns>The new Command object with the specified arguments.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithArguments(IEnumerable<string> arguments, bool escapeArguments)
        {
            string args = string.Join(" ", arguments);

            if (escapeArguments)
            {
                args = ArgumentsBuilder.Escape(args);
            }

            return new Command(TargetFilePath,
                args,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);
        }

        /// <summary>
        /// Sets the arguments to pass to the executable.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the executable.</param>
        /// <returns>The new Command object with the specified arguments.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithArguments(string arguments) =>
            new Command(TargetFilePath,
                arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the Target File Path of the Command Executable.
        /// </summary>
        /// <param name="targetFilePath">The target file path of the Command.</param>
        /// <remarks>Usage of this builder method is usually unnecessary as Command's constructor requires the Target File Path to be passed to it. </remarks>
        /// <returns>The Command with the updated Target File Path.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithTargetFile(string targetFilePath) =>
            new Command(targetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);


        /// <summary>
        /// Sets the environment variables to be configured.
        /// </summary>
        /// <param name="environmentVariables">The environment variables to be configured.</param>
        /// <returns>The new Command with the specified environment variables.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                environmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the environment variables for the Command to be executed.
        /// </summary>
        /// <param name="configure">The environment variables to be configured</param>
        /// <returns>The new Command with the specified environment variables.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
        {
            EnvironmentVariablesBuilder environmentVariablesBuilder = new EnvironmentVariablesBuilder()
                .Set(EnvironmentVariables);

            configure(environmentVariablesBuilder);

            return WithEnvironmentVariables(environmentVariablesBuilder.Build());
        }

        /// <summary>
        /// Sets whether to execute the Command with Administrator Privileges.
        /// </summary>
        /// <param name="runAsAdministrator">Whether to execute the Command with Administrator Privileges.</param>
        /// <returns>The new Command with the specified Administrator Privileges settings.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithAdministratorPrivileges(bool runAsAdministrator) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                runAsAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the working directory to be used for the Command.
        /// </summary>
        /// <param name="workingDirectoryPath">The working directory to be used.</param>
        /// <returns>The new Command with the specified working directory.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithWorkingDirectory(string workingDirectoryPath) =>
            new Command(TargetFilePath,
                Arguments,
                workingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);


        /// <summary>
        /// Sets the specified Credentials to be used.
        /// </summary>
        /// <param name="credentials">The credentials to be used.</param>
        /// <returns>The new Command with the specified Credentials.</returns>
        /// <remarks>Credentials are only supported with the Process class on Windows. This is a limitation of .NET's Process class.</remarks>
        [Pure]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
#endif
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithCredentials(UserCredentials credentials) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the credentials for the Command to be executed.
        /// </summary>
        /// <param name="configure">The CredentialsBuilder configuration.</param>
        /// <returns>The new Command with the specified Credentials.</returns>
        /// <remarks>Credentials are only supported with the Process class on Windows. This is a limitation of .NET's Process class.</remarks>
        [Pure]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
#endif
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithCredentials(Action<CredentialsBuilder> configure)
        {
            CredentialsBuilder credentialBuilder = new CredentialsBuilder()
                .SetDomain(Credentials.Domain)
                .SetPassword(Credentials.Password)
                .SetUsername(Credentials.UserName);

            configure(credentialBuilder);

            return WithCredentials(credentialBuilder.Build());
        }

        /// <summary>
        /// Sets the Result Validation whether to throw an exception or not if the Command does not execute successfully.
        /// </summary>
        /// <param name="validation">The result validation behaviour to be used.</param>
        /// <returns>The new Command object with the configured Result Validation behaviour.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithValidation(CommandResultValidation validation) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                validation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the Standard Input Pipe source.
        /// </summary>
        /// <param name="source">The source to use for the Standard Input pipe.</param>
        /// <returns>The new Command with the specified Standard Input pipe source.</returns>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror"/>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithStandardInputPipe(StreamWriter source) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                source,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the Standard Output Pipe target.
        /// </summary>
        /// <param name="target">The target to send the Standard Output to.</param>
        /// <returns>The new Command with the specified Standard Output Pipe Target.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithStandardOutputPipe(StreamReader target) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                target,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the Standard Error Pipe target.
        /// </summary>
        /// <param name="target">The target to send the Standard Error to.</param>
        /// <returns>The new Command with the specified Standard Error Pipe Target.</returns>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithStandardErrorPipe(StreamReader target) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                target,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the Processor Affinity for this command.
        /// </summary>
        /// <param name="processorAffinity">The processor affinity to use.</param>
        /// <returns>The new Command with the specified Processor Affinity.</returns>
        [Pure]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithProcessorAffinity(IntPtr processorAffinity) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                processorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Enables or disables command execution via Shell Execution.
        /// </summary>
        /// <param name="useShellExecution">Whether to enable or disable shell execution.</param>
        /// <returns>The new Command with the specified shell execution behaviour.</returns>
        /// <remarks>Using Shell Execution whilst also Redirecting Standard Input will throw an Exception. This is a known issue with the System Process class.</remarks>
        /// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandarderror"/>
        [Pure]
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithShellExecution(bool useShellExecution) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                useShellExecution);

        /// <summary>
        /// Enables or disables Window creation for the wrapped executable.
        /// </summary>
        /// <param name="enableWindowCreation">Whether to enable or disable window creation for the wrapped executable.</param>
        /// <returns>The new Command with the specified window creation behaviour.</returns>
        [Pure] 
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithWindowCreation(bool enableWindowCreation) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                StandardInputEncoding,
                StandardOutputEncoding,
                StandardErrorEncoding,
                ProcessorAffinity,
                enableWindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the Encoding types to be used for Standard Input, Output, and Error.
        /// </summary>
        /// <param name="standardInputEncoding">The encoding type to be used for the Standard Input.</param>
        /// <param name="standardOutputEncoding">The encoding type to be used for the Standard Output.</param>
        /// <param name="standardErrorEncoding">The encoding type to be used for the Standard Error.</param>
        /// <returns>The new Command with the specified Pipe Encoding types.</returns>
        [Pure] 
        [Obsolete("Command's Builder methods are deprecated and will be removed in a future version. Use CommandBuilder instead.")]
        public Command WithEncoding(Encoding standardInputEncoding = default,
            Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RequiresAdministrator,
                EnvironmentVariables,
                Credentials,
                ResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
                standardInputEncoding,
                standardOutputEncoding,
                standardErrorEncoding,
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);


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