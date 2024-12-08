/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using CliRunner.Builders;
using CliRunner.Commands.Abstractions;
using CliRunner.Piping;
using CliRunner.Piping.Abstractions;

namespace CliRunner.Commands
{
    public partial class Command : ICommandConfiguration
    {
        public bool RunAsAdministrator { get; protected set; }
        public string TargetFilePath { get; protected set; }
        public string WorkingDirectoryPath { get; protected set; }
        public string Arguments { get; protected set; }
        
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }
        public Credentials Credentials { get; protected set; }
        public CliRunner.Commands.CommandResultValidation CommandResultValidation { get; protected set;}
        
        public PipeSource StandardInputPipe { get; protected set; }
        public PipeTarget StandardOutputPipe { get; protected set; }
        public PipeTarget StandardErrorPipe { get; protected set; }

        public Command(string targetFilePath,
             string arguments = null, string workingDirectoryPath = null,
             bool runAsAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
             Credentials credentials = null,
             CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
             PipeSource standardInputPipe = null,
             PipeTarget standardOutputPipe = null,
             PipeTarget standardErrorPipe = null
        )
        {
            TargetFilePath = targetFilePath;
            RunAsAdministrator = runAsAdministrator;
            Arguments = arguments ?? string.Empty;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credentials = credentials ?? Credentials.Default;

            CommandResultValidation = commandResultValidation;
            
            StandardInputPipe = standardInputPipe ?? StandardInputPipe!.Null;
            StandardOutputPipe = standardOutputPipe ?? StandardOutputPipe!.Null;
            StandardErrorPipe = standardErrorPipe ?? StandardErrorPipe!.Null;
        }
        
        public Command WithArguments(IEnumerable<string> arguments) =>
            new Command(TargetFilePath,
                string.Join(" ", arguments),
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);

        public Command WithArguments(IEnumerable<string> arguments, bool escape)
        {
            return new Command(TargetFilePath,
                string.Join(" ", arguments),
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);
        }
        
        public Command WithArguments(string arguments)=> 
            new Command(TargetFilePath,
                arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);
        
        
        public Command WithTargetFile(string targetFilePath) => 
            new Command(targetFilePath,
            Arguments,
            WorkingDirectoryPath,
            RunAsAdministrator,
            EnvironmentVariables,
            Credentials,
            CommandResultValidation,
            StandardInputPipe,
            StandardOutputPipe,
            StandardErrorPipe);
        
        
        public Command WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                environmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);

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
            StandardInputPipe,
            StandardOutputPipe,
            StandardErrorPipe);
        
        
        public Command WithWorkingDirectory(string workingDirectoryPath) =>
            new Command(TargetFilePath,
                Arguments,
                workingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);
        
        
        public Command WithCredentials(Credentials credentials) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);

        public Command WithCredentials(Action<CredentialsBuilder> configure)
        {
           var credentialBuilder = new CredentialsBuilder().SetDomain(Credentials.Domain)
                .SetPassword(Credentials.Password)
                .SetUsername(Credentials.Username);

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
                StandardInputPipe,
                StandardOutputPipe,
                StandardErrorPipe);
        
        public Command WithStandardInputPipe(PipeSource source) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                source,
                StandardOutputPipe,
                StandardErrorPipe);
        
        public Command WithStandardOutputPipe(PipeTarget target) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                target,
                StandardErrorPipe);
        
        public Command WithStandardErrorPipe(PipeTarget target) =>
            new Command(TargetFilePath,
                Arguments,
                WorkingDirectoryPath,
                RunAsAdministrator,
                EnvironmentVariables,
                Credentials,
                CommandResultValidation,
                StandardInputPipe,
                StandardOutputPipe,
                target);
        
        public override string ToString()
        {
            return base.ToString();
        }
    }
}