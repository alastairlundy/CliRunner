/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Collections.Generic;

using CliRunner.Piping.Abstractions;
using CliRunner.Processes;

namespace CliRunner.Commands
{
    public class Command : ICommandConfiguration
    {
        public string TargetFilePath { get; protected set; }
        public string WorkingDirectoryPath { get; protected set; }
        public string[] Arguments { get; protected set; }
        
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; protected set; }
        public Credentials Credentials { get; protected set; }
        public CommandResultValidation ProcessResultValidator { get; protected set;}
        
        public AbstractPipeSource StandardInputPipe { get; protected set; }
        public AbstractPipeTarget StandardOutputPipe { get; protected set; }
        public AbstractPipeTarget StandardErrorPipe { get; protected set; }
        
        public Command(string targetFilePath)
        {
            
        }
        
        
    }
}