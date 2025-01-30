/*
    CliRunner
    Copyright (C) 2024-2025  Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CliRunner.Builders;

namespace CliRunner.Abstractions;

/// <summary>
/// This interface details the fluent builder methods all CommandBuilders must implement. 
/// </summary>
public interface ICommandBuilder
{
    ICommandBuilder WithArguments(IEnumerable<string> arguments);
    ICommandBuilder WithArguments(IEnumerable<string> arguments, bool escape);

    ICommandBuilder WithArguments(string arguments);
    ICommandBuilder WithTargetFile(string targetFilePath);
    ICommandBuilder WithEnvironmentVariables(IReadOnlyDictionary<string, string> environmentVariables);
    ICommandBuilder WithEnvironmentVariables(Action<EnvironmentVariablesBuilder> configure);

    ICommandBuilder WithAdministratorPrivileges(bool runAsAdministrator);

    ICommandBuilder WithWorkingDirectory(string workingDirectoryPath);
    ICommandBuilder WithCredentials(UserCredentials credentials);
    ICommandBuilder WithCredentials(Action<CredentialsBuilder> configure);
    ICommandBuilder WithValidation(CommandResultValidation validation);
    ICommandBuilder WithStandardInputPipe(StreamWriter source);
    ICommandBuilder WithStandardOutputPipe(StreamReader target);
    ICommandBuilder WithStandardErrorPipe(StreamReader target);
    ICommandBuilder WithProcessorAffinity(IntPtr processorAffinity);
    ICommandBuilder WithShellExecution(bool useShellExecute);
    ICommandBuilder WithWindowCreation(bool useWindowCreation);

    ICommandBuilder WithEncoding(Encoding standardInputEncoding = default,
        Encoding standardOutputEncoding = default,
        Encoding standardErrorEncoding = default);
    
    Command Build();
}