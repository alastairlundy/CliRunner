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
using System.Text;
using CliRunner.Builders;

namespace CliRunner.Abstractions
{
    public interface ICommandConfigurationBuilder
    {
        ICommand WithArguments(IEnumerable<string> arguments);
        ICommand WithArguments(IEnumerable<string> arguments, bool escape);

        ICommand WithArguments(string arguments);
        ICommand WithTargetFile(string targetFilePath);
        ICommand WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables);
        ICommand WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure);

        ICommand WithAdministratorPrivileges(bool runAsAdministrator);

        ICommand WithWorkingDirectory(string workingDirectoryPath);
        ICommand WithCredentials(UserCredentials credentials);
        ICommand WithCredentials(Action<CredentialsBuilder> configure);
        ICommand WithValidation(CommandResultValidation validation);
        ICommand WithStandardInputPipe(StreamWriter source);
        ICommand WithStandardOutputPipe(StreamReader target);
        ICommand WithStandardErrorPipe(StreamReader target);
        ICommand WithProcessorAffinity(IntPtr processorAffinity);
        ICommand WithShellExecution(bool useShellExecute);
        ICommand WithWindowCreation(bool useWindowCreation);

        ICommand WithEncoding(Encoding standardInputEncoding = default,
            Encoding standardOutputEncoding = default,
            Encoding standardErrorEncoding = default);
    }
}