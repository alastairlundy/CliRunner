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

using CliRunner.Builders;
using CliRunner.Commands.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandConfiguration
    {
        public bool RunAsAdministrator { get; protected set; }
        public string TargetFilePath { get; protected set; }
        public string WorkingDirectoryPath { get; protected set; }
        public string Arguments { get; protected set; }
        
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }
        public CliRunner.Commands.CommandResultValidation CommandResultValidation { get; protected set;}
        public UserCredentials Credentials { get; protected set; }
        
        public StreamWriter StandardInput{ get; protected set; }
        public StreamReader StandardOutput { get; protected set; }
        public StreamReader StandardError { get; protected set; }

        public Command(string targetFilePath,
             string arguments = null, string workingDirectoryPath = null,
             bool runAsAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
             UserCredentials credentials = null,
             CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
             StreamWriter standardInput = null,
             StreamReader standardOutput = null,
             StreamReader standardError = null,
        )
        {
            TargetFilePath = targetFilePath;
            RunAsAdministrator = runAsAdministrator;
            Arguments = arguments ?? string.Empty;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credentials = credentials ?? UserCredentials.Default;

            CommandResultValidation = commandResultValidation;
            StandardInput = standardInput ?? StreamWriter.Null;
            StandardOutput = standardOutput ?? StreamReader.Null;
            StandardError = standardError ?? StreamReader.Null;
            
        }
        
        public Command WithArguments(IEnumerable<string> arguments) =>
            new Command(TargetFilePath,
                string.Join(" ", arguments),
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,

        public Command WithArguments(IEnumerable<string> arguments, bool escape)
        {
            return new Command(TargetFilePath,
                string.Join(" ", arguments),
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
        }
        
        public Command WithArguments(string arguments)=> 
            new Command(TargetFilePath,
                arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
        
        public Command WithTargetFile(string targetFilePath) => 
            new Command(targetFilePath,
            Arguments,
            WorkingDirectoryPath,
            RunAsAdministrator,
            EnvironmentVariables,
            Credentials,
            CommandResultValidation,
            StandardInput,
            StandardOutput,
            StandardError,
        
        
        public Command WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                environmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,

        public Command WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure)
        {
            var environmentVariablesBuilder = new EnvironmentVariablesBuilder()
                .Set(EnvironmentVariables);

            configure(environmentVariablesBuilder);
           
            return WithEnvironmentVariables(environmentVariablesBuilder.Build());
        }
        
        public Command RequiresAdministrator(bool runAsAdministrator) =>
            new Command(TargetFilePath,
            Arguments,
            WorkingDirectoryPath,
            runAsAdministrator,
            EnvironmentVariables,
            Credentials,
            CommandResultValidation,
            StandardInput,
            StandardOutput,
            StandardError,
        
        public Command WithWorkingDirectory(string workingDirectoryPath) =>
            new Command(TargetFilePath,
                Arguments,
                workingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,
        
        
        public Command WithCredentials(Credentials credentials) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                StandardError,

        public Command WithCredentials(Action<CredentialsBuilder> configure)
        {
            var credentialBuilder = new CredentialsBuilder().SetDomain(Credentials.Domain)
                .SetPassword(Credentials.Password)

           configure(credentialBuilder);
           
           return WithCredentials(credentialBuilder.Build());
        }

        public Command WithValidation(CommandResultValidation validation) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                validation,
                StandardInput,
                StandardOutput,
                StandardError,
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Command WithStandardInput(StreamWriter source) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                source,
                StandardOutput,
                StandardError,
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Command WithStandardOutputStream(StreamReader target) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                target,
                StandardError,
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Command WithStandardErrorStream(StreamReader target) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInput,
                StandardOutput,
                target,
        
    }
}