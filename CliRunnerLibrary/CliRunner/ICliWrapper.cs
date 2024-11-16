/*
    CliRunner 
    Copyright (C) 2024  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System.Threading.Tasks;
using CliRunner.Commands;
using CliRunner.Processes;
using CliRunner.Processes.Abstractions;

namespace CliRunner
{
    public interface ICliWrapper
    {
        ICliWrapper Wrap(string pathToExecutable);
        ICliWrapper WithArguments(params string[] args);
        ICliWrapper WithWorkingDirectory(string directory);
        ICliWrapper WithEnvironmentVariables(params string[] variables);
        ICliWrapper WithCredentials();
        
        ProcessResult Execute();
        Task<ProcessResult> ExecuteAsync();
    }
}