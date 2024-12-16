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

namespace CliRunner.Commands.Abstractions
{
    public interface ICommandConfigurationBuilder
    {
        Command WithArguments(IEnumerable<string> arguments);
        Command WithArguments(IEnumerable<string> arguments, bool escape);

        Command WithArguments(string arguments);
        Command WithTargetFile(string targetFilePath);
        Command WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables);
        Command WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure);

        Command RunAsAdministrator(bool runAsAdministrator);

        Command WithWorkingDirectory(string workingDirectoryPath);
        Command WithCredentials(UserCredentials credentials);
        Command WithCredentials(Action<CredentialsBuilder> configure);
        Command WithValidation(CommandResultValidation validation);
        Command WithStandardInputPipe(StreamWriter source);
        Command WithStandardOutputPipe(StreamReader target);
        Command WithStandardErrorPipe(StreamReader target);
        Command WithProcessorAffinity(IntPtr processorAffinity);
        Command WithShellExecute(bool useShellExecute);

    }
}