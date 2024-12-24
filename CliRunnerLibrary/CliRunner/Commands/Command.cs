/*
    CliRunner
    Copyright (C) 2024  Alastair Lundy

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
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Versioning;

using CliRunner.Builders;
using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandConfiguration, ICommandConfigurationBuilder
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
        
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }
        
        /// <summary>
        /// The credentials to be used when executing the executable.
        /// </summary>
        public UserCredentials Credentials { get; protected set; }
        
        /// <summary>
        /// The result validation to apply to the Command when it is executed.
        /// </summary>
        public CommandResultValidation ResultValidation { get; protected set;}
        
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
        /// <param name="processorAffinity">The processor affinity to be used (if specified).</param>
        /// <param name="windowCreation">Whether to enable or disable Window Creation by the Command's Process.</param>
        /// <param name="useShellExecute">Whether to enable or disable executing the Command through Shell Execution.</param>
        public Command(string targetFilePath,
             string arguments = null, string workingDirectoryPath = null,
             bool runAsAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
             UserCredentials credentials = null,
             CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
             StreamWriter standardInput = null,
             StreamReader standardOutput = null,
             StreamReader standardError = null,
#if NET6_0_OR_GREATER
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
        }
        
        /// <summary>
        /// Sets the arguments to pass to the executable.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the executable.</param>
        /// <returns>The new Command object with the specified arguments.</returns>
        [Pure]
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the arguments to pass to the executable.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the executable.</param>
        /// <param name="escape">Whether to escape the arguments if escape characters are detected.</param>
        /// <returns>The new Command object with the specified arguments.</returns>
        [Pure]
        public Command WithArguments(IEnumerable<string> arguments, bool escape)
        {
            string args = string.Join(" ", arguments);
            
            if (escape)
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
        [Pure]
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
            ProcessorAffinity,
            WindowCreation,
            UseShellExecution);
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentVariables">The environment variables to be configured.</param>
        /// <returns></returns>
        [Pure]
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the environment variables for the Command to be executed.
        /// </summary>
        /// <param name="configure">The environment variables to be configured</param>
        /// <returns></returns>
        [Pure]
        public Command WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
        {
            var environmentVariablesBuilder = new EnvironmentVariablesBuilder()
                .Set(EnvironmentVariables);

            configure(environmentVariablesBuilder);
           
            return WithEnvironmentVariables(environmentVariablesBuilder.Build());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runAsAdministrator"></param>
        /// <returns></returns>
        [Pure]
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
            ProcessorAffinity,
            WindowCreation,
            UseShellExecution);
        
        /// <summary>
        /// Sets the working directory to be used for the Command.
        /// </summary>
        /// <param name="workingDirectoryPath">The working directory to be used.</param>
        /// <returns>The new Command with the specified working directory.</returns>
        [Pure]
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);
        
        
        /// <summary>
        /// Sets the specified Credentials to be used.
        /// </summary>
        /// <param name="credentials">The credentials to be used.</param>
        /// <returns>The new Command with the specified Credentials.</returns>
        /// <remarks>Credentials are only supported with the Process class on Windows.</remarks>
        [Pure]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
#endif
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);

        /// <summary>
        /// Sets the credentials for the Command to be executed.
        /// </summary>
        /// <param name="configure">The CredentialsBuilder configuration.</param>
        /// <returns>The new Command with the specified Credentials.</returns>
        /// <remarks>Credentials are only supported with the Process class on Windows.</remarks>
        [Pure]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
#endif
        public Command WithCredentials(Action<CredentialsBuilder> configure)
        {
            var credentialBuilder = new CredentialsBuilder().SetDomain(Credentials.Domain)
                .SetPassword(Credentials.Password)
                .SetUsername(Credentials.UserName);

           configure(credentialBuilder);
           
           return WithCredentials(credentialBuilder.Build());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validation"></param>
        /// <returns></returns>
        [Pure]
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [Pure]
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
                ProcessorAffinity,
                WindowCreation,
                UseShellExecution);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [Pure]
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
                ProcessorAffinity,
                WindowCreation,
                useShellExecution);
        
        public Command WithWindowCreation(bool useShellExecution) =>
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
                ProcessorAffinity,
                WindowCreation,
                useShellExecution);
    }
}