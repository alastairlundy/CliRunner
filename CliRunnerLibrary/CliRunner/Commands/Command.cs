/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

using CliRunner.Commands.Abstractions;
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
        
        public AbstractPipeSource StandardInputPipe { get; protected set; }
        public AbstractPipeTarget StandardOutputPipe { get; protected set; }
        public AbstractPipeTarget StandardErrorPipe { get; protected set; }

        public Command(string targetFilePath,
             string arguments = null, string workingDirectoryPath = null,
             bool runAsAdministrator = false,
            IReadOnlyDictionary<string, string> environmentVariables = null,
             Credentials credentials = null,
             CommandResultValidation commandResultValidation = CommandResultValidation.ExitCodeZero,
             AbstractPipeSource standardInputPipe = null,
             AbstractPipeTarget standardOutputPipe = null,
             AbstractPipeTarget standardErrorPipe = null
        )
        {
            TargetFilePath = targetFilePath;
            RunAsAdministrator = runAsAdministrator;
            Arguments = arguments ?? string.Empty;
            WorkingDirectoryPath = workingDirectoryPath ?? Directory.GetCurrentDirectory();
            EnvironmentVariables = environmentVariables ?? new Dictionary<string, string>();
            Credentials = credentials ?? Credentials.Default;

            CommandResultValidation = commandResultValidation;
            
            StandardInputPipe = standardInputPipe ?? StandardInputPipe.Null;
            StandardOutputPipe = standardOutputPipe ?? StandardOutputPipe.Null;
            StandardErrorPipe = standardErrorPipe ?? StandardErrorPipe.Null;
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
    }
}