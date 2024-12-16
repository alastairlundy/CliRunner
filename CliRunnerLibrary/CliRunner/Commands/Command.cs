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
using System.IO;

using CliRunner.Builders;
using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandConfiguration
    {
        public bool RequiresAdministrator { get; protected set; }
        public string TargetFilePath { get; protected set; }
        public string WorkingDirectoryPath { get; protected set; }
        public string Arguments { get; protected set; }
        
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }
        public UserCredentials Credentials { get; protected set; }
        public CommandResultValidation ResultValidation { get; protected set;}
        
        public StreamWriter StandardInput { get; protected set; }
        public StreamReader StandardOutput { get; protected set; }
        public StreamReader StandardError { get; protected set; }
        
        public IntPtr ProcessorAffinity { get; protected set; }
        
        public bool UseShellExecute { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <param name="arguments"></param>
        /// <param name="workingDirectoryPath"></param>
        /// <param name="runAsAdministrator"></param>
        /// <param name="environmentVariables"></param>
        /// <param name="credentials"></param>
        /// <param name="commandResultValidation"></param>
        /// <param name="standardInput"></param>
        /// <param name="standardOutput"></param>
        /// <param name="standardError"></param>
        /// <param name="processorAffinity"></param>
        /// <param name="useShellExecute"></param>
        public Command(string targetFilePath,
             string arguments = null, string workingDirectoryPath = null,
             bool runAsAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
             UserCredentials credentials = null,
             CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
             StreamWriter standardInput = null,
             StreamReader standardOutput = null,
             StreamReader standardError = null,
             IntPtr processorAffinity = default(IntPtr),
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
            UseShellExecute = useShellExecute;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
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
                UseShellExecute);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="escape"></param>
        /// <returns></returns>
        public Command WithArguments(IEnumerable<string> arguments, bool escape)
        {
            return new Command(TargetFilePath,
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
                UseShellExecute);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public Command WithArguments(string arguments)=> 
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
                UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <returns></returns>
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
            UseShellExecute);
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentVariables"></param>
        /// <returns></returns>
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
                UseShellExecute);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
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
        public Command RunAsAdministrator(bool runAsAdministrator) =>
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
            UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDirectoryPath"></param>
        /// <returns></returns>
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
                UseShellExecute);
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
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
                UseShellExecute);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
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
                UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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
                UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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
                UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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
                UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processorAffinity"></param>
        /// <returns></returns>
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
                UseShellExecute);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useShellExecute"></param>
        /// <returns></returns>
        public Command WithShellExecute(bool useShellExecute) =>
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
                useShellExecute);
    }
}